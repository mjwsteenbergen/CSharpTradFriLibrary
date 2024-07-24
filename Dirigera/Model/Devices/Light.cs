using Newtonsoft.Json;

namespace Tomidix.NetStandard.Dirigera.Devices;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

public class Light : DirigeraDevice
{
    [JsonProperty("attributes")]
    public LightAttributes Attributes { get; set; }

    [JsonProperty("room")]
    public Room Room { get; set; }
    public override string GetName() => Attributes.CustomName;

    public override string ToString()
    {
        var state = Attributes.IsOn ? Attributes.LightLevel.ToString() : "Off";
        return Attributes.CustomName + $"[{state}]";
    }

    public Task Toggle()
    {
        return Service.DeviceController.Toggle(this) ?? Task.CompletedTask;
    }

    public Task SetLightLevel(int lightLevel)
    {
        return Service.DeviceController.SetLightLevel(this, lightLevel) ?? Task.CompletedTask;
    }

    public Task SetLightTemperature(int colorTemperature)
    {
        return Service.DeviceController.SetLightTemperature(this, colorTemperature) ?? Task.CompletedTask;
    }
}

public class LightAttributes : Attributes
{

    [JsonProperty("isOn")]
    public bool IsOn { get; set; }

    [JsonProperty("startupOnOff")]
    public string StartupOnOff { get; set; }

    [JsonProperty("lightLevel")]
    public long LightLevel { get; set; }

    [JsonProperty("startUpCurrentLevel")]
    public long StartUpCurrentLevel { get; set; }

    [JsonProperty("colorMode")]
    public string ColorMode { get; set; }

    [JsonProperty("startupTemperature")]
    public long StartupTemperature { get; set; }

    [JsonProperty("colorTemperature")]
    public long ColorTemperature { get; set; }

    [JsonProperty("colorTemperatureMax")]
    public long ColorTemperatureMax { get; set; }

    [JsonProperty("colorTemperatureMin")]
    public long ColorTemperatureMin { get; set; }
}