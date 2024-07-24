using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using ApiLibs.General;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tomidix.NetStandard.Dirigera.Controller;

namespace Tomidix.NetStandard.Dirigera.Devices;

public class DeviceConverter : JsonConverter<DirigeraDevice>

{
    public override DirigeraDevice ReadJson(JsonReader reader, Type objectType, [AllowNull] DirigeraDevice existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JToken jObject = JToken.ReadFrom(reader);

        string type = "";

        try
        {
            if (jObject.Type is not JTokenType.None and not JTokenType.Null)
            {
                type = jObject["deviceType"]?.ToObject<string>() ?? "";
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
            _ => throw new ArgumentOutOfRangeException("Cannot convert type " + type + "\n" + jObject.ToString())
        };


        serializer.Populate(jObject.CreateReader(), result);
        return result;
    }

    public override bool CanWrite => false;

    public override void WriteJson(JsonWriter writer, [AllowNull] DirigeraDevice value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

[JsonConverter(typeof(DeviceConverter))]
public abstract class DirigeraDevice : ObjectSearcher<DirigeraController>
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("deviceType")]
    public string DeviceType { get; set; }

    [JsonProperty("createdAt")]
    public string CreatedAt { get; set; }

    [JsonProperty("isReachable")]
    public bool IsReachable { get; set; }

    [JsonProperty("lastSeen")]
    public string LastSeen { get; set; }

    public abstract string GetName();
}

public partial class Attributes
{
    [JsonProperty("customName")]
    public string CustomName { get; set; }

    [JsonProperty("model")]
    public string Model { get; set; }

    [JsonProperty("manufacturer")]
    public string Manufacturer { get; set; }

    [JsonProperty("firmwareVersion")]
    public string FirmwareVersion { get; set; }

    [JsonProperty("hardwareVersion")]
    public string HardwareVersion { get; set; }

    [JsonProperty("serialNumber")]
    public string SerialNumber { get; set; }

    [JsonProperty("productCode")]
    public string ProductCode { get; set; }

    [JsonProperty("identifyPeriod")]
    public long IdentifyPeriod { get; set; }

    [JsonProperty("identifyStarted")]
    public DateTimeOffset IdentifyStarted { get; set; }

    [JsonProperty("permittingJoin")]
    public bool PermittingJoin { get; set; }

    [JsonProperty("otaPolicy")]
    public string OtaPolicy { get; set; }

    [JsonProperty("otaProgress")]
    public long OtaProgress { get; set; }

    [JsonProperty("otaScheduleEnd")]
    public string OtaScheduleEnd { get; set; }

    [JsonProperty("otaScheduleStart")]
    public string OtaScheduleStart { get; set; }

    [JsonProperty("otaState")]
    public string OtaState { get; set; }

    [JsonProperty("otaStatus")]
    public string OtaStatus { get; set; }
}