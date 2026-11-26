using ApiLibs.JSON.Serialization;
using System.Text.Json;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Tomidix.NetStandard.Dirigera.Devices;

public class LightAttributeConverter : JsonConverter<LightAttributes>
{
    public override LightAttributes Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        JsonElement json;
        using (var document = JsonDocument.ParseValue(ref reader))
        {
            json = document.RootElement.Clone();
        }

        LightAttributes result = json.ValueKind == JsonValueKind.Object && json.TryGetProperty("colorMode", out _)
            ? new TemperatureLightAttributes()
            : new LightAttributes();

        JsonCompat.Populate(json, result);
        return result;
    }

    public override void Write(Utf8JsonWriter writer, LightAttributes value, JsonSerializerOptions options)
    {
        JsonCompat.WriteDefault(writer, value, options);
    }
}

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

public class Light : DirigeraDevice
{
    [JsonPropertyName("attributes")]
    public LightAttributes Attributes { get; set; }

    [JsonPropertyName("room")]
    public Room Room { get; set; }
    public override string GetName() => Attributes.CustomName;

    public override string ToString()
    {
        var state = Attributes.IsOn ? Attributes.LightLevel.ToString() : "Off";
        return Attributes.CustomName + $"[{state}]";
    }

    public Task Toggle()
    {
        return Service.DeviceController.Toggle(this);
    }

    public Task SetLightLevel(int lightLevel)
    {
        return Service.DeviceController.SetLightLevel(this, lightLevel);
    }

    public Task SetLightTemperature(int colorTemperature)
    {
        return Service.DeviceController.SetLightTemperature(this, colorTemperature);
    }
}

[JsonConverter(typeof(LightAttributeConverter))]
public class LightAttributes : Attributes
{

    [JsonPropertyName("isOn")]
    public bool IsOn { get; set; }

    [JsonPropertyName("startupOnOff")]
    public string StartupOnOff { get; set; }

    [JsonPropertyName("lightLevel")]
    public long LightLevel { get; set; }

    [JsonPropertyName("startUpCurrentLevel")]
    public long StartUpCurrentLevel { get; set; }
}

public class TemperatureLightAttributes : LightAttributes
{
    [JsonPropertyName("colorMode")]
    public string ColorMode { get; set; }

    [JsonPropertyName("startupTemperature")]
    public long StartupTemperature { get; set; }

    [JsonPropertyName("colorTemperature")]
    public long ColorTemperature { get; set; }

    [JsonPropertyName("colorTemperatureMax")]
    public long ColorTemperatureMax { get; set; }

    [JsonPropertyName("colorTemperatureMin")]
    public long ColorTemperatureMin { get; set; }
}

public class LightEventAttributes : EventAttributes
{
    [JsonPropertyName("isOn")]
    public bool? IsOn { get; set; }

    [JsonPropertyName("startupOnOff")]
    public string? StartupOnOff { get; set; }

    [JsonPropertyName("lightLevel")]
    public long? LightLevel { get; set; }

    [JsonPropertyName("startUpCurrentLevel")]
    public long? StartUpCurrentLevel { get; set; }

    [JsonPropertyName("colorMode")]
    public string? ColorMode { get; set; }

    [JsonPropertyName("startupTemperature")]
    public long? StartupTemperature { get; set; }

    [JsonPropertyName("colorTemperature")]
    public long? ColorTemperature { get; set; }

    [JsonPropertyName("colorTemperatureMax")]
    public long? ColorTemperatureMax { get; set; }

    [JsonPropertyName("colorTemperatureMin")]
    public long? ColorTemperatureMin { get; set; }
}