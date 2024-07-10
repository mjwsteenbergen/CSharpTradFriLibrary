using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tomidix.NetStandard.Dirigera.Devices;

namespace Tomidix.NetStandard.Dirigera.Model.Attributes;

public class DirigeraAttributeConverter : JsonConverter<DirigeraAttribute>

{
    public override DirigeraAttribute ReadJson(JsonReader reader, Type objectType, [AllowNull] DirigeraAttribute existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JToken jToken = JToken.ReadFrom(reader);

        if (jToken is JObject jObject)
        {
            DirigeraAttribute result = jObject.Properties().First().Name switch
            {
                "isOn" => new ToggleAttribute(),
                "lightLevel" => new LightLevelAttribute(),
                "colorTemperature" => new ColorTemperatureAttribute(),
                "currentRH" => new RelativeHumidityAttribute(),
                "vocIndex" => new VOCAttribute(),
                "currentPM25" => new Pm25Attribute(),
                "illuminance" => new IlluminanceAttribute(),
                "isDetected" => new MotionAttribute(),
                _ => new UnknownAttribute
                {
                    Json = jToken.ToString(Formatting.Indented)
                }
            };
            serializer.Populate(jToken.CreateReader(), result);
            return result;

        }

        return new UnknownAttribute
        {
            Json = jToken.ToString(Formatting.None)
        };

    }

    public override bool CanWrite => false;
    public override void WriteJson(JsonWriter writer, [AllowNull] DirigeraAttribute value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}

[JsonConverter(typeof(DirigeraAttributeConverter))]
public interface DirigeraAttribute
{

}

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

public class ToggleAttribute : DirigeraAttribute
{
    [JsonProperty("isOn")]
    public bool IsOn { get; set; }
}


public class LightLevelAttribute : DirigeraAttribute
{
    [JsonProperty("lightLevel")]
    public int LightLevel { get; set; }
}

public class ColorTemperatureAttribute : DirigeraAttribute
{
    [JsonProperty("colorTemperature")]
    public int ColorTemperature { get; set; }
}

public class RelativeHumidityAttribute : DirigeraAttribute
{
    [JsonProperty("currentRH")]
    public int CurrentRelativeHumidity { get; set; }
}

public class VOCAttribute : DirigeraAttribute
{
    [JsonProperty("vocIndex")]
    public int VocIndex { get; set; }
}

public class Pm25Attribute : DirigeraAttribute
{
    [JsonProperty("currentPM25")]
    public int CurrentPM25 { get; set; }
}

public class IlluminanceAttribute : DirigeraAttribute
{
    [JsonProperty("illuminance")]
    public int Illuminance { get; set; }
}

public class MotionAttribute : DirigeraAttribute
{
    [JsonProperty("isDetected")]
    public bool IsDetected { get; set; }

    [JsonProperty("sensorConfig")]
    public SensorConfig SensorConfig { get; set; }

    [JsonProperty("circadianPresets")]
    public object[] CircadianPresets { get; set; }
}

public class MotionDetectedDelayAttribute : DirigeraAttribute
{
    [JsonProperty("motionDetectedDelay")]
    public int MotionDetectedDelay { get; set; }
}

public class UnknownAttribute : DirigeraAttribute
{
    public string Json { get; set; }
}

