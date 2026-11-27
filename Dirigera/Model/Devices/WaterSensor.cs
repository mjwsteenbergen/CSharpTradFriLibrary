using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Tomidix.NetStandard.Dirigera.Devices;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.


public class WaterSensor : DirigeraDevice
{
    [JsonPropertyName("attributes")]
    public WaterSensorAttributes Attributes { get; set; }

    [JsonPropertyName("room")]
    public Room Room { get; set; }
    public override string GetName() => Attributes.CustomName;

    public override string ToString()
    {
        var state = Attributes.WaterLeakDetected ? "WATER DETECTED" : "Fine";
        return Attributes.CustomName + $"[{state}]";
    }
}

public class WaterSensorAttributes : Attributes
{
    [JsonPropertyName("waterLeakDetected")]
    public bool WaterLeakDetected { get; set; }

    [JsonPropertyName("batteryPercentage")]
    public int BatteryPercentage { get; set; }
}

public class WaterSensorEventAttributes : EventAttributes
{
    [JsonPropertyName("waterLeakDetected")]
    public bool? WaterLeakDetected { get; set; }

    [JsonPropertyName("batteryPercentage")]
    public int? BatteryPercentage { get; set; }
}