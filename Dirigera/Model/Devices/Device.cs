using ApiLibs.JSON.Serialization;
using System.Text.Json;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using ApiLibs.JSON.General;
using System.Text.Json.Serialization;
using Tomidix.NetStandard.Dirigera.Controller;

namespace Tomidix.NetStandard.Dirigera.Devices;

public class DeviceConverter : JsonConverter<DirigeraDevice>
{
    public override DirigeraDevice Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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
                type = JsonCompat.ValueAsString(json.GetProperty("deviceType")) ?? "";
            }
        }
        catch { }

        DirigeraDevice result = type switch
        {
            "environmentSensor" => new EnvironmentSensor(),
            "gateway" => new Gateway(),
            "light" => new Light(),
            "motionSensor" => new MotionSensor(),
            "lightSensor" => new LightSensor(),
            "outlet" => new Outlet(),
            "waterSensor" => new WaterSensor(),
            "electricalSensor" => new ElectricalSensor(),
            _ => throw new ArgumentOutOfRangeException("Cannot convert type " + type + "\n" + json.GetRawText())
        };

        JsonCompat.Populate(json, result);
        return result;
    }

    public override void Write(Utf8JsonWriter writer, DirigeraDevice value, JsonSerializerOptions options)
    {
        JsonCompat.WriteDefault(writer, value, options);
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