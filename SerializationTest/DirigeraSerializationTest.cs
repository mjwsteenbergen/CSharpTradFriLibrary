using System.Text.Json;
using Tomidix.NetStandard.Dirigera;
using NUnit.Framework;
using Tomidix.NetStandard.Dirigera.Devices;
using Tomidix.NetStandard.Dirigera.Model.Events;

namespace SerializationTest;

/// <summary>Reads hub payloads with <see cref="DirigeraJson.Options"/>, as DirigeraController and EventController do.</summary>
public class DirigeraSerializationTest
{
    private static T Read<T>(string json) => JsonSerializer.Deserialize<T>(json, DirigeraJson.Options)!;

    private const string Devices = """
        [
          {
            "id": "light-1", "type": "light", "deviceType": "light", "createdAt": "2024-01-01T10:00:00.000Z",
            "isReachable": true, "lastSeen": "2024-06-01T10:00:00.000Z",
            "attributes": {
              "customName": "Desk", "model": "TRADFRI bulb", "isOn": true, "lightLevel": 80,
              "colorMode": "temperature", "colorTemperature": 2700, "colorTemperatureMin": 4000, "colorTemperatureMax": 2202
            },
            "room": { "id": "room-1", "name": "Office" }
          },
          {
            "id": "light-2", "type": "light", "deviceType": "light", "isReachable": true,
            "attributes": { "customName": "Hallway", "isOn": false, "lightLevel": 10 }
          },
          {
            "id": "water-1", "type": "sensor", "deviceType": "waterSensor", "isReachable": true,
            "attributes": { "customName": "Kitchen sink", "waterLeakDetected": false, "batteryPercentage": 87 }
          }
        ]
        """;

    [Test]
    public void DevicesAreReadAsTheirSubclass()
    {
        var devices = Read<List<DirigeraDevice>>(Devices);

        Assert.That(devices, Has.Count.EqualTo(3));
        var desk = (Light)devices[0];
        Assert.That(desk.Id, Is.EqualTo("light-1"));
        Assert.That(desk.Attributes, Is.TypeOf<TemperatureLightAttributes>(), "colorMode picks the temperature attributes");
        Assert.That(((TemperatureLightAttributes)desk.Attributes).ColorTemperature, Is.EqualTo(2700));
        Assert.That(desk.Attributes.LightLevel, Is.EqualTo(80));
        Assert.That(desk.Room.Name, Is.EqualTo("Office"));

        var hallway = (Light)devices[1];
        Assert.That(hallway.Attributes, Is.TypeOf<LightAttributes>());
        Assert.That(hallway.Attributes.IsOn, Is.False);

        var water = (WaterSensor)devices[2];
        Assert.That(water.Attributes.BatteryPercentage, Is.EqualTo(87));
    }

    [Test]
    public void ASingleDeviceCanBeReadAsItsSubclass()
    {
        var light = Read<Light>("""{ "id": "light-2", "deviceType": "light", "attributes": { "isOn": true } }""");

        Assert.That(light.Attributes.IsOn, Is.True);
    }

    [Test]
    public void AnUnknownDeviceTypeIsRejected()
    {
        Assert.That(() => Read<DirigeraDevice>("""{ "id": "x", "deviceType": "blinds" }"""),
            Throws.InstanceOf<JsonException>());
    }

    [Test]
    public void ADeviceIsWrittenWithItsOwnMembers()
    {
        var light = (Light)Read<List<DirigeraDevice>>(Devices)[0];

        var json = JsonSerializer.Serialize(light, DirigeraJson.Options);

        Assert.That(json, Does.Contain("\"id\":\"light-1\""));
        Assert.That(json, Does.Contain("\"colorTemperature\":2700"));
    }

    [Test]
    public void AWaterLeakEventCarriesWaterSensorAttributes()
    {
        var e = Read<DirigeraEvent>("""
            {
              "id": "evt-1", "time": "2024-06-01T10:00:00.000Z", "specversion": "1.1.0", "source": "urn:com:ikea:homesmart:iotc:ipdevice",
              "type": "deviceStateChanged",
              "data": { "id": "water-1", "deviceType": "waterSensor", "isReachable": true, "attributes": { "waterLeakDetected": true } }
            }
            """);

        var changed = (DirigeraStateChangedEvent)e;
        Assert.That(changed.Data.Id, Is.EqualTo("water-1"));
        Assert.That(changed.Data.Attributes, Is.TypeOf<WaterSensorEventAttributes>());
        Assert.That(((WaterSensorEventAttributes)changed.Data.Attributes).WaterLeakDetected, Is.True);
        Assert.That(((WaterSensorEventAttributes)changed.Data.Attributes).BatteryPercentage, Is.Null);
    }

    [Test]
    public void AnEventForAnUnknownDeviceKeepsItsAttributesAsJson()
    {
        var e = (DirigeraStateChangedEvent)Read<DirigeraEvent>("""
            { "id": "evt-2", "type": "deviceStateChanged", "data": { "id": "b-1", "deviceType": "blinds", "attributes": { "customName": "Bedroom", "blindsCurrentLevel": 40 } } }
            """);

        var attributes = (UnknownEventAttributes)e.Data.Attributes;
        Assert.That(attributes.DeviceType, Is.EqualTo("blinds"));
        Assert.That(attributes.CustomName, Is.EqualTo("Bedroom"));
        Assert.That(attributes.Json, Does.Contain("\"blindsCurrentLevel\": 40"));
    }

    [Test]
    public void PongAndUnknownEventsAreRecognised()
    {
        Assert.That(Read<DirigeraEvent>("""{ "type": "pong", "data": { "lastModified": "x" } }"""), Is.TypeOf<DirigeraPongEvent>());

        var unknown = Read<DirigeraEvent>("""{ "type": "sceneUpdated", "data": { "id": "scene-1" } }""");
        Assert.That(unknown, Is.TypeOf<UnknownEvent>());
        Assert.That(((UnknownEvent)unknown).Type, Is.EqualTo("sceneUpdated"));
    }

    [Test]
    public void DevicesCanAlsoBeReadWithDefaultOptions()
    {
        var devices = JsonSerializer.Deserialize<List<DirigeraDevice>>(Devices)!;

        Assert.That(devices[0], Is.TypeOf<Light>());
        Assert.That(((Light)devices[0]).Attributes, Is.TypeOf<TemperatureLightAttributes>());
    }

    [Test]
    public void APingIsWrittenAsAFlatEvent()
    {
        var json = JsonSerializer.Serialize<DirigeraEvent>(new DirigeraPingEvent { Id = "1", Type = "ping", Specversion = "1.1.0", Source = "urn:test" }, DirigeraJson.Options);

        Assert.That(json, Does.StartWith("{\"id\":\"1\""));
        Assert.That(json, Does.Contain("\"type\":\"ping\""));
    }

    [Test]
    public void AStateChangedEventSurvivesARoundTrip()
    {
        var e = Read<DirigeraEvent>("""
            { "id": "evt-1", "type": "deviceStateChanged", "data": { "id": "water-1", "deviceType": "waterSensor", "attributes": { "waterLeakDetected": true, "batteryPercentage": 40 } } }
            """);

        var again = (DirigeraStateChangedEvent)Read<DirigeraEvent>(JsonSerializer.Serialize(e, DirigeraJson.Options));

        Assert.That(again.Data.Id, Is.EqualTo("water-1"));
        var attributes = (WaterSensorEventAttributes)again.Data.Attributes;
        Assert.That(attributes.WaterLeakDetected, Is.True);
        Assert.That(attributes.BatteryPercentage, Is.EqualTo(40));
    }
}
