using ApiLibs.JSON.Serialization;
using System.Text.Json;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Tomidix.NetStandard.Dirigera.Devices;

namespace Tomidix.NetStandard.Dirigera.Model.Events;

public class EventConverter : JsonConverter<DirigeraEvent>
{
    public override DirigeraEvent Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        JsonElement json;
        using (var document = JsonDocument.ParseValue(ref reader))
        {
            json = document.RootElement.Clone();
        }

        string type = "";

        try
        {
            if (json.ValueKind is not JsonValueKind.Undefined and not JsonValueKind.Null)
            {
                type = JsonCompat.ValueAsString(json.GetProperty("type")) ?? "";
            }
        }
        catch { }

        DirigeraEvent result = type switch
        {
            "deviceStateChanged" => new DirigeraStateChangedEvent(),
            "pong" => new DirigeraPongEvent(),
            _ => new UnknownEvent()
        };

        JsonCompat.Populate(json, result);
        return result;
    }

    public override void Write(Utf8JsonWriter writer, DirigeraEvent value, JsonSerializerOptions options)
    {
        JsonCompat.WriteDefault(writer, value, options);
    }
}

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

[JsonConverter(typeof(EventConverter))]
public class DirigeraEvent
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("time")]
    public DateTimeOffset Time { get; set; }

    [JsonPropertyName("specversion")]
    public string Specversion { get; set; }

    [JsonPropertyName("source")]
    public string Source { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }
}

public class UnknownEvent : DirigeraEvent
{
    [JsonPropertyName("data")]
    public dynamic Data { get; set; }
}

public class DirigeraStateChangedEvent : DirigeraEvent
{
    [JsonPropertyName("data")]
    public DirigeraStateChangedEventData Data { get; set; }
}

public class DirigeraPongEvent : DirigeraEvent
{
    [JsonPropertyName("data")]
    public DirigeraPongEventData Data { get; set; }
}


public class DirigeraPongEventData
{
    [JsonPropertyName("lastModified")]
    public string LastModified { get; set; }
}

[JsonConverter(typeof(DirigeraStateChangedEventDataConverter))]
public class DirigeraStateChangedEventData : EventDirigeraDevice
{
    [JsonPropertyName("attributes")]
    public EventAttributes Attributes { get; set; }
}

public class DirigeraStateChangedEventDataConverter : JsonConverter<DirigeraStateChangedEventData>
{
    private static readonly JsonSerializerOptions Indented = new() { WriteIndented = true };

    public override DirigeraStateChangedEventData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        JsonElement json;
        using (var document = JsonDocument.ParseValue(ref reader))
        {
            json = document.RootElement.Clone();
        }

        string deviceType = json.TryGetProperty("deviceType", out var deviceTypeToken) ? JsonCompat.ValueAsString(deviceTypeToken) ?? "" : "";
        bool hasAttributes = json.TryGetProperty("attributes", out var attributesToken);

        EventAttributes attributes = deviceType switch
        {
            "environmentSensor" => new EnvironmentSensorEventAttributes(),
            "light" => new LightEventAttributes(),
            "motionSensor" => new MotionSensorEventAttributes(),
            "lightSensor" => new LightSensorEventAttributes(),
            "outlet" => new OutletEventAttributes(),
            "waterSensor" => new WaterSensorEventAttributes(),
            "electricalSensor" => new ElectricalSensorEventAttributes(),
            _ => new UnknownEventAttributes
            {
                DeviceType = deviceType,
                Json = hasAttributes ? JsonSerializer.Serialize(attributesToken, Indented) : null!
            }
        };

        if (hasAttributes)
            JsonCompat.Populate(attributesToken, attributes);

        // Populate all other properties on DirigeraEvent automatically
        var result = new DirigeraStateChangedEventData { Attributes = attributes };
        JsonCompat.Populate(json, result);

        return result;
    }

    public override void Write(Utf8JsonWriter writer, DirigeraStateChangedEventData value, JsonSerializerOptions options)
    {
        JsonCompat.WriteDefault(writer, value, options);
    }
}

public class UnknownEventAttributes : EventAttributes
{
    public required string DeviceType { get; set; }
    public required string Json { get; set; }
}

