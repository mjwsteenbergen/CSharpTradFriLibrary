using System.Reflection;
using Tomidix.NetStandard.Tradfri.Extensions;
using NUnit.Framework;
using Tomidix.NetStandard.Tradfri.Models;

namespace SerializationTest;

public class TradfriSerializationTest
{
    [Test]
    public async Task EpochSecondsAndMicrosecondsAreBothRead()
    {
        var device = await FakeService.Read<TradfriDevice>("""{ "9001": "Bulb", "9003": 65537, "9002": 1700000000, "9020": 1700000000123456 }""");

        Assert.That(device.Name, Is.EqualTo("Bulb"));
        Assert.That(device.ID, Is.EqualTo(65537));
        Assert.That(device.CreatedAt, Is.EqualTo(new DateTime(2023, 11, 14, 22, 13, 20, DateTimeKind.Utc)));
        Assert.That(device.LastSeen, Is.EqualTo(new DateTime(2023, 11, 14, 22, 13, 20, 123, DateTimeKind.Utc)));
    }

    [Test]
    public async Task ANullEpochIsMinValue()
    {
        var device = await FakeService.Read<TradfriDevice>("""{ "9002": null }""");

        Assert.That(device.CreatedAt, Is.EqualTo(DateTime.MinValue));
    }

    [Test]
    public void RequestBodiesLeaveOutNulls()
    {
        // CoapImplementation.AddBody is what turns request content into the CoAP payload.
        var addBody = typeof(CoapImplementation).GetMethod("AddBody", BindingFlags.NonPublic | BindingFlags.Static)!;
        var json = (string)addBody.Invoke(null, new object[] { new LightControl { State = Bool.True } })!;

        Assert.That(json, Does.Contain("\"5850\":1"));
        Assert.That(json, Does.Not.Contain("null"));
    }
}
