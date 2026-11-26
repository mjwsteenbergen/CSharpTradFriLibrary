using System.Text.Json.Serialization;

namespace Tomidix.NetStandard.Dirigera.Devices;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

public class MotionSensor : DirigeraDevice
{
    [JsonPropertyName("attributes")]
    public MotionSensorAttributes Attributes { get; set; }

    [JsonPropertyName("room")]
    public Room Room { get; set; }

    public Task SetMotionDetectedDelay(int delay)
    {
        return Service.DeviceController.SetMotionDetectedDelay(this, delay);
    }

    public override string GetName() => Attributes.CustomName;

    public override string ToString()
    {
        var state = Attributes.IsDetected ? "Seen motion" : "No Motion";
        return Attributes.CustomName + $"[{state}]";
    }
}

public class MotionSensorAttributes : Attributes
{
    [JsonPropertyName("batteryPercentage")]
    public long BatteryPercentage { get; set; }

    [JsonPropertyName("isOn")]
    public bool IsOn { get; set; }

    [JsonPropertyName("isDetected")]
    public bool IsDetected { get; set; }

    [JsonPropertyName("motionDetectedDelay")]
    public long MotionDetectedDelay { get; set; }

    [JsonPropertyName("sensorConfig")]
    public SensorConfig SensorConfig { get; set; }

    [JsonPropertyName("circadianPresets")]
    public object[] CircadianPresets { get; set; }

    [JsonPropertyName("lightLevel")]
    public int? LightLevel { get; set; }
}

public partial class SensorConfig
{
    [JsonPropertyName("scheduleOn")]
    public bool ScheduleOn { get; set; }

    [JsonPropertyName("onDuration")]
    public long OnDuration { get; set; }

    [JsonPropertyName("schedule")]
    public Schedule Schedule { get; set; }
}

public partial class Schedule
{
    [JsonPropertyName("onCondition")]
    public Condition OnCondition { get; set; }

    [JsonPropertyName("offCondition")]
    public Condition OffCondition { get; set; }
}

public partial class Condition
{
    [JsonPropertyName("time")]
    public string Time { get; set; }
}

public class MotionSensorEventAttributes : EventAttributes
{
    [JsonPropertyName("batteryPercentage")]
    public long? BatteryPercentage { get; set; }

    [JsonPropertyName("isOn")]
    public bool? IsOn { get; set; }

    [JsonPropertyName("isDetected")]
    public bool? IsDetected { get; set; }

    [JsonPropertyName("motionDetectedDelay")]
    public long? MotionDetectedDelay { get; set; }

    [JsonPropertyName("sensorConfig")]
    public SensorConfig? SensorConfig { get; set; }

    [JsonPropertyName("circadianPresets")]
    public object[]? CircadianPresets { get; set; }

    [JsonPropertyName("lightLevel")]
    public int? LightLevel { get; set; }
}