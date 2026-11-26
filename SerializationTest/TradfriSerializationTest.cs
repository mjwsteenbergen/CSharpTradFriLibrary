using ApiLibs.JSON.Serialization;
using NUnit.Framework;
using Tomidix.NetStandard.Tradfri.Models;

namespace SerializationTest;

public class TradfriSerializationTest
{
    [Test]
    public void EpochSecondsAndMicrosecondsAreBothRead()
    {
        var device = JsonCompat.Deserialize<TradfriDevice>("""{ "9001": "Bulb", "9003": 65537, "9002": 1700000000, "9020": 1700000000123456 }""");

        Assert.That(device.Name, Is.EqualTo("Bulb"));
        Assert.That(device.ID, Is.EqualTo(65537));
        Assert.That(device.CreatedAt, Is.EqualTo(new DateTime(2023, 11, 14, 22, 13, 20, DateTimeKind.Utc)));
        Assert.That(device.LastSeen, Is.EqualTo(new DateTime(2023, 11, 14, 22, 13, 20, 123, DateTimeKind.Utc)));
    }

    [Test]
    public void ANullEpochIsMinValue()
    {
        var device = JsonCompat.Deserialize<TradfriDevice>("""{ "9002": null }""");

        Assert.That(device.CreatedAt, Is.EqualTo(DateTime.MinValue));
    }
}
