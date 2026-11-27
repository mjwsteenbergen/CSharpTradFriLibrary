using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Tomidix.NetStandard.Dirigera.Devices;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.


public class Outlet : DirigeraDevice
{
    [JsonPropertyName("attributes")]
    public OutletAttributes Attributes { get; set; }

    [JsonPropertyName("room")]
    public Room Room { get; set; }
    public override string GetName() => Attributes.CustomName;

    public override string ToString()
    {
        var state = Attributes.IsOn ? "On" : "Off";
        return Attributes.CustomName + $"[{state}]";
    }

    public Task Toggle()
    {
        return Service.DeviceController.Toggle(this);
    }
}

public class OutletAttributes : Attributes
{
    [JsonPropertyName("isOn")]
    public bool IsOn { get; set; }

    [JsonPropertyName("startupOnOff")]
    public string StartupOnOff { get; set; }

    [JsonPropertyName("lightLevel")]
    public long LightLevel { get; set; }

    [JsonPropertyName("startUpCurrentLevel")]
    public long StartUpCurrentLevel { get; set; }

    [JsonPropertyName("currentVoltage")]
    public long CurrentVoltage { get; set; }

    [JsonPropertyName("currentAmps")]
    public long CurrentAmps { get; set; }

    [JsonPropertyName("currentActivePower")]
    public long CurrentActivePower { get; set; }

    [JsonPropertyName("totalEnergyConsumed")]
    public long TotalEnergyConsumed { get; set; }

    [JsonPropertyName("totalEnergyConsumedLastUpdated")]
    public DateTime TotalEnergyConsumedLastUpdated { get; set; }

    [JsonPropertyName("energyConsumedAtLastReset")]
    public long EnergyConsumedAtLastReset { get; set; }

    [JsonPropertyName("timeOfLastEnergyReset")]
    public DateTime TimeOfLastEnergyReset { get; set; }

    [JsonPropertyName("childLock")]
    public bool ChildLock { get; set; }

    [JsonPropertyName("statusLight")]
    public bool StatusLight { get; set; }
}

public class OutletEventAttributes : EventAttributes
{
    [JsonPropertyName("isOn")]
    public bool? IsOn { get; set; }

    [JsonPropertyName("startupOnOff")]
    public string? StartupOnOff { get; set; }

    [JsonPropertyName("lightLevel")]
    public long? LightLevel { get; set; }

    [JsonPropertyName("startUpCurrentLevel")]
    public long? StartUpCurrentLevel { get; set; }

    [JsonPropertyName("currentVoltage")]
    public double? CurrentVoltage { get; set; }

    [JsonPropertyName("currentAmps")]
    public double? CurrentAmps { get; set; }

    [JsonPropertyName("currentActivePower")]
    public double? CurrentActivePower { get; set; }

    [JsonPropertyName("totalEnergyConsumed")]
    public double? TotalEnergyConsumed { get; set; }

    [JsonPropertyName("totalEnergyConsumedLastUpdated")]
    public DateTime? TotalEnergyConsumedLastUpdated { get; set; }

    [JsonPropertyName("energyConsumedAtLastReset")]
    public double? EnergyConsumedAtLastReset { get; set; }

    [JsonPropertyName("timeOfLastEnergyReset")]
    public DateTime? TimeOfLastEnergyReset { get; set; }

    [JsonPropertyName("childLock")]
    public bool? ChildLock { get; set; }

    [JsonPropertyName("statusLight")]
    public bool? StatusLight { get; set; }
}