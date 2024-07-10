using System.Diagnostics.CodeAnalysis;
using ApiLibs.MicrosoftGraph;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tomidix.NetStandard.Dirigera.Model.Attributes;

namespace Tomidix.NetStandard.Dirigera.Model.Events;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

public class DirigeraEvent
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("time")]
    public DateTimeOffset Time { get; set; }

    [JsonProperty("specversion")]
    public string Specversion { get; set; }

    [JsonProperty("source")]
    public string Source { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("data")]
    public DirigeraEventDevice Data { get; set; }
}

public class DirigeraEventDevice : Device
{
    [JsonProperty("attributes")]
    public DirigeraAttribute Attributes { get; set; }
}

