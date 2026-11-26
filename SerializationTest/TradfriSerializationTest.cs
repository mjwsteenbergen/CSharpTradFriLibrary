using System.Text.Json;
using Tomidix.NetStandard.Tradfri.Extensions;
using NUnit.Framework;
using Tomidix.NetStandard.Tradfri.Models;

namespace SerializationTest;

public class TradfriSerializationTest
{
    [Test]
    public void EpochSecondsAndMicrosecondsAreBothRead()
    {
        var device = JsonSerializer.Deserialize<TradfriDevice>("""{ "9001": "Bulb", "9003": 65537, "9002": 1700000000, "9020": 1700000000123456 }""", TradfriJson.Options)!;

        Assert.That(device.Name, Is.EqualTo("Bulb"));
        Assert.That(device.ID, Is.EqualTo(65537));
        Assert.That(device.CreatedAt, Is.EqualTo(new DateTime(2023, 11, 14, 22, 13, 20, DateTimeKind.Utc)));
        Assert.That(device.LastSeen, Is.EqualTo(new DateTime(2023, 11, 14, 22, 13, 20, 123, DateTimeKind.Utc)));
    }

    [Test]
    public void ANullEpochIsMinValue()
    {
        var device = JsonSerializer.Deserialize<TradfriDevice>("""{ "9002": null }""", TradfriJson.Options)!;

        Assert.That(device.CreatedAt, Is.EqualTo(DateTime.MinValue));
    }

    [Test]
    public void RequestBodiesLeaveOutNulls()
    {
        var json = JsonSerializer.Serialize(new LightControl { State = Bool.True }, TradfriJson.Options);

        Assert.That(json, Does.Contain("\"5850\":1"));
        Assert.That(json, Does.Not.Contain("null"));
    }
}
