using System.Text.Json.Serialization;

namespace Tomidix.NetStandard.Dirigera.Devices;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

public class EnvironmentSensor : DirigeraDevice
{
    [JsonPropertyName("attributes")]
    public EnvironmentSensorAttributes Attributes { get; set; }

    [JsonPropertyName("room")]
    public Room Room { get; set; }

    public override string GetName() => Attributes.CustomName;

    public override string ToString()
    {
        return Attributes.CustomName;
    }
}

public partial class Room
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("color")]
    public string Color { get; set; }

    [JsonPropertyName("icon")]
    public string Icon { get; set; }
}

public class EnvironmentSensorAttributes : Attributes
{
    [JsonPropertyName("currentTemperature")]
    public int CurrentTemperature { get; set; }

    [JsonPropertyName("currentRH")]
    public int CurrentRH { get; set; }

    [JsonPropertyName("currentPM25")]
    public int CurrentPM25 { get; set; }

    [JsonPropertyName("maxMeasuredPM25")]
    public int MaxMeasuredPM25 { get; set; }

    [JsonPropertyName("minMeasuredPM25")]
    public int MinMeasuredPM25 { get; set; }

    [JsonPropertyName("vocIndex")]
    public int VocIndex { get; set; }

    [JsonPropertyName("currentCO2")]
    public int? CurrentCO2 { get; set; }
}

public class EnvironmentSensorEventAttributes : EventAttributes
{
    [JsonPropertyName("currentTemperature")]
    public int? CurrentTemperature { get; set; }

    [JsonPropertyName("currentRH")]
    public int? CurrentRH { get; set; }

    [JsonPropertyName("currentPM25")]
    public int? CurrentPM25 { get; set; }

    [JsonPropertyName("maxMeasuredPM25")]
    public int? MaxMeasuredPM25 { get; set; }

    [JsonPropertyName("minMeasuredPM25")]
    public int? MinMeasuredPM25 { get; set; }

    [JsonPropertyName("vocIndex")]
    public int? VocIndex { get; set; }

    [JsonPropertyName("currentCO2")]
    public int? CurrentCO2 { get; set; }
}