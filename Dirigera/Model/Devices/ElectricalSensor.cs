using Newtonsoft.Json;

namespace Tomidix.NetStandard.Dirigera.Devices;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.


/// <summary>
/// The power meter of a metering plug such as the GRILLPLATS. The hub lists it as its own
/// device next to the <see cref="Outlet"/>, sharing the outlet's relationId and custom name.
/// </summary>
public class ElectricalSensor : DirigeraDevice
{
    [JsonProperty("relationId")]
    public string RelationId { get; set; }

    [JsonProperty("attributes")]
    public ElectricalSensorAttributes Attributes { get; set; }

    [JsonProperty("room")]
    public Room Room { get; set; }
    public override string GetName() => Attributes.CustomName;

    public override string ToString()
    {
        return Attributes.CustomName + $"[{Attributes.CurrentActivePower}W]";
    }
}

public class ElectricalSensorAttributes : Attributes
{
    [JsonProperty("currentActivePower")]
    public double CurrentActivePower { get; set; }

    [JsonProperty("currentAmps")]
    public double CurrentAmps { get; set; }

    [JsonProperty("currentVoltage")]
    public double CurrentVoltage { get; set; }

    [JsonProperty("totalEnergyConsumed")]
    public double TotalEnergyConsumed { get; set; }

    [JsonProperty("totalEnergyConsumedLastUpdated")]
    public DateTime TotalEnergyConsumedLastUpdated { get; set; }

    [JsonProperty("energyConsumedAtLastReset")]
    public double EnergyConsumedAtLastReset { get; set; }

    [JsonProperty("timeOfLastEnergyReset")]
    public DateTime TimeOfLastEnergyReset { get; set; }
}

/// <summary>
/// The hub sends the same metering fields an outlet sends, so anything that reads an outlet's
/// power readings picks these up as well.
/// </summary>
public class ElectricalSensorEventAttributes : OutletEventAttributes
{
}
