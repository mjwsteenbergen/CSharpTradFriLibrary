using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Tomidix.NetStandard.Dirigera.Devices;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.


public class Outlet : DirigeraDevice
{
    [JsonProperty("attributes")]
    public OutletAttributes Attributes { get; set; }

    [JsonProperty("room")]
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
    [JsonProperty("isOn")]
    public bool IsOn { get; set; }

    [JsonProperty("startupOnOff")]
    public string StartupOnOff { get; set; }
}