using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Tomidix.NetStandard.Dirigera.Devices;

namespace Tomidix.NetStandard.Dirigera.Model.Events;

/// <summary>Reads an event as the subclass its <c>type</c> names; any other type is an <see cref="UnknownEvent"/>.</summary>
public class EventConverter : JsonConverter<DirigeraEvent>
{
    public override DirigeraEvent? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var document = JsonDocument.ParseValue(ref reader);
        var json = document.RootElement;

        string? type = json.ValueKind == JsonValueKind.Object && json.TryGetProperty("type", out var value) && value.ValueKind == JsonValueKind.String
            ? value.GetString()
            : null;

        // The subclasses have no converter of their own, so these read them member by member.
        return type switch
        {
            "deviceStateChanged" => json.Deserialize<DirigeraStateChangedEvent>(options),
            "pong" => json.Deserialize<DirigeraPongEvent>(options),
            _ => json.Deserialize<UnknownEvent>(options)
        };
    }

    public override void Write(Utf8JsonWriter writer, DirigeraEvent value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

[JsonConverter(typeof(EventConverter))]
public abstract class DirigeraEvent
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
    [JsonConverter(typeof(DirigeraStateChangedEventDataConverter))]
    public DirigeraStateChangedEventData Data { get; set; }
}

/// <summary>Sent to the hub to keep the websocket open; it answers with a <see cref="DirigeraPongEvent"/>.</summary>
public class DirigeraPingEvent : DirigeraEvent
{
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

public class DirigeraStateChangedEventData : EventDirigeraDevice
{
    [JsonPropertyName("attributes")]
    public EventAttributes Attributes { get; set; }
}

/// <summary>
/// Reads <see cref="DirigeraStateChangedEventData.Attributes"/> as the subclass for the device's
/// <c>deviceType</c>, which sits next to the attributes rather than inside them. Applied to
/// <see cref="DirigeraStateChangedEvent.Data"/> rather than to the type, so reading the rest of the
/// data does not come back here.
/// </summary>
public class DirigeraStateChangedEventDataConverter : JsonConverter<DirigeraStateChangedEventData>
{
    private static readonly JsonSerializerOptions Indented = new() { WriteIndented = true };

    public override DirigeraStateChangedEventData? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var data = JsonNode.Parse(ref reader)?.AsObject();
        if (data == null)
        {
            return null;
        }

        string? deviceType = data["deviceType"] is JsonValue value && value.TryGetValue(out string? text) ? text : null;
        JsonNode? attributesJson = data["attributes"];
        data.Remove("attributes");

        // EventAttributes is abstract, so the attributes are left out here and read below.
        var result = data.Deserialize<DirigeraStateChangedEventData>(options)!;
        result.Attributes = ReadAttributes(deviceType, attributesJson ?? new JsonObject(), options);
        return result;
    }

    private static EventAttributes ReadAttributes(string? deviceType, JsonNode json, JsonSerializerOptions options)
    {
        Type? type = deviceType switch
        {
            "environmentSensor" => typeof(EnvironmentSensorEventAttributes),
            "light" => typeof(LightEventAttributes),
            "motionSensor" => typeof(MotionSensorEventAttributes),
            "lightSensor" => typeof(LightSensorEventAttributes),
            "outlet" => typeof(OutletEventAttributes),
            "waterSensor" => typeof(WaterSensorEventAttributes),
            "electricalSensor" => typeof(ElectricalSensorEventAttributes),
            _ => null
        };

        if (type != null)
        {
            return (EventAttributes)json.Deserialize(type, options)!;
        }

        var unknown = json.Deserialize<UnknownEventAttributes>(options)!;
        unknown.DeviceType = deviceType ?? "";
        unknown.Json = json.ToJsonString(Indented);
        return unknown;
    }

    public override void Write(Utf8JsonWriter writer, DirigeraStateChangedEventData value, JsonSerializerOptions options)
    {
        // Attributes is declared as the abstract base; write it as what it actually is.
        var data = JsonSerializer.SerializeToNode(value, options)!.AsObject();
        data["attributes"] = value.Attributes == null ? null : JsonSerializer.SerializeToNode(value.Attributes, value.Attributes.GetType(), options);
        data.WriteTo(writer, options);
    }
}

/// <summary>Attributes of a device type this library does not model. <see cref="Json"/> holds them as sent.</summary>
public class UnknownEventAttributes : EventAttributes
{
    [JsonIgnore]
    public string DeviceType { get; set; } = "";

    [JsonIgnore]
    public string Json { get; set; } = "";
}
