using System.Text.Json;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using ApiLibs.JSON.General;
using System.Text.Json.Serialization;
using Tomidix.NetStandard.Dirigera.Controller;

namespace Tomidix.NetStandard.Dirigera.Devices;

/// <summary>Reads a device as the subclass its <c>deviceType</c> names.</summary>
public class DeviceConverter : JsonConverter<DirigeraDevice>
{
    public override DirigeraDevice? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var document = JsonDocument.ParseValue(ref reader);
        var json = document.RootElement;

        string? deviceType = json.ValueKind == JsonValueKind.Object && json.TryGetProperty("deviceType", out var value) && value.ValueKind == JsonValueKind.String
            ? value.GetString()
            : null;

        Type type = deviceType switch
        {
            "environmentSensor" => typeof(EnvironmentSensor),
            "gateway" => typeof(Gateway),
            "light" => typeof(Light),
            "motionSensor" => typeof(MotionSensor),
            "lightSensor" => typeof(LightSensor),
            "outlet" => typeof(Outlet),
            "waterSensor" => typeof(WaterSensor),
            "electricalSensor" => typeof(ElectricalSensor),
            _ => throw new JsonException($"Unknown deviceType '{deviceType}': {json.GetRawText()}")
        };

        // The subclasses have no converter of their own, so this reads them member by member.
        return (DirigeraDevice?)json.Deserialize(type, options);
    }

    public override void Write(Utf8JsonWriter writer, DirigeraDevice value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.


[JsonConverter(typeof(DeviceConverter))]
public abstract class DirigeraDevice : ObjectSearcher<DirigeraController>
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("deviceType")]
    public string DeviceType { get; set; }

    [JsonPropertyName("createdAt")]
    public string CreatedAt { get; set; }

    [JsonPropertyName("isReachable")]
    public bool IsReachable { get; set; }

    [JsonPropertyName("lastSeen")]
    public string LastSeen { get; set; }

    public abstract string GetName();
}

public class EventDirigeraDevice
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("deviceType")]
    public string DeviceType { get; set; }

    [JsonPropertyName("createdAt")]
    public string CreatedAt { get; set; }

    [JsonPropertyName("isReachable")]
    public bool IsReachable { get; set; }

    [JsonPropertyName("lastSeen")]
    public string LastSeen { get; set; }
}

public partial class Attributes
{
    [JsonPropertyName("customName")]
    public string CustomName { get; set; }

    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("manufacturer")]
    public string Manufacturer { get; set; }

    [JsonPropertyName("firmwareVersion")]
    public string FirmwareVersion { get; set; }

    [JsonPropertyName("hardwareVersion")]
    public string HardwareVersion { get; set; }

    [JsonPropertyName("serialNumber")]
    public string SerialNumber { get; set; }

    [JsonPropertyName("productCode")]
    public string ProductCode { get; set; }

    [JsonPropertyName("identifyPeriod")]
    public long IdentifyPeriod { get; set; }

    [JsonPropertyName("identifyStarted")]
    public DateTimeOffset IdentifyStarted { get; set; }

    [JsonPropertyName("permittingJoin")]
    public bool PermittingJoin { get; set; }

    [JsonPropertyName("otaPolicy")]
    public string OtaPolicy { get; set; }

    [JsonPropertyName("otaProgress")]
    public long OtaProgress { get; set; }

    [JsonPropertyName("otaScheduleEnd")]
    public string OtaScheduleEnd { get; set; }

    [JsonPropertyName("otaScheduleStart")]
    public string OtaScheduleStart { get; set; }

    [JsonPropertyName("otaState")]
    public string OtaState { get; set; }

    [JsonPropertyName("otaStatus")]
    public string OtaStatus { get; set; }
}

public abstract class EventAttributes
{
    [JsonPropertyName("customName")]
    public string? CustomName { get; set; }

    [JsonPropertyName("model")]
    public string? Model { get; set; }

    [JsonPropertyName("manufacturer")]
    public string? Manufacturer { get; set; }

    [JsonPropertyName("firmwareVersion")]
    public string? FirmwareVersion { get; set; }

    [JsonPropertyName("hardwareVersion")]
    public string? HardwareVersion { get; set; }

    [JsonPropertyName("serialNumber")]
    public string? SerialNumber { get; set; }

    [JsonPropertyName("productCode")]
    public string? ProductCode { get; set; }

    [JsonPropertyName("identifyPeriod")]
    public long? IdentifyPeriod { get; set; }

    [JsonPropertyName("identifyStarted")]
    public DateTimeOffset? IdentifyStarted { get; set; }

    [JsonPropertyName("permittingJoin")]
    public bool? PermittingJoin { get; set; }

    [JsonPropertyName("otaPolicy")]
    public string? OtaPolicy { get; set; }

    [JsonPropertyName("otaProgress")]
    public long? OtaProgress { get; set; }

    [JsonPropertyName("otaScheduleEnd")]
    public string? OtaScheduleEnd { get; set; }

    [JsonPropertyName("otaScheduleStart")]
    public string? OtaScheduleStart { get; set; }

    [JsonPropertyName("otaState")]
    public string? OtaState { get; set; }

    [JsonPropertyName("otaStatus")]
    public string? OtaStatus { get; set; }
}