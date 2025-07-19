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
            string propertyName = jObject.Properties().First().Name;
            DirigeraAttribute result = propertyName switch
            {
                "isOn" => new ToggleAttribute(),
                "lightLevel" => new LightLevelAttribute(),
                "colorTemperature" => new ColorTemperatureAttribute(),
                "colorMode" => new ColorTemperatureAttribute(),
                "currentTemperature" => new TemperatureAttribute(),
                "currentRH" => new RelativeHumidityAttribute(),
                "vocIndex" => new VOCAttribute(),
                "currentPM25" => new Pm25Attribute(),
                "illuminance" => new IlluminanceAttribute(),
                "isDetected" => new MotionAttribute(),
                "otaState" => new OTAStateAttribute(),
                "waterLeakDetected" => new WaterLeakDetectedAttribute(),
                "currentAmps" => new CurrentPowerUsageAttribute(),
                "currentVoltage" => new CurrentPowerUsageAttribute(),
                "currentActivePower" => new CurrentPowerUsageAttribute(),
                "totalEnergyConsumed" => new TotalEnergyConsumedAttribute(),
                "totalEnergyConsumedLastUpdated" => new TotalEnergyConsumedAttribute(),
                "otaStatus" => new OtaStatusAttribute(),
                "otaProgress" => new OtaProgressAttribute(),
                "identifyPeriod" => new IdentifyAttribute(),
                "batteryPercentage" => new BatteryAttribute(),
                _ => new UnknownAttribute
                {
                    MissingPropertyName = propertyName,
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


public class OTAStateAttribute : DirigeraAttribute
{
    [JsonProperty("otaState")]
    public string OtaState { get; set; }

    [JsonProperty("sensorConfig")]
    public SensorConfig SensorConfig { get; set; }

    [JsonProperty("circadianPresets")]
    public object[] CircadianPresets { get; set; }
}

public class SensorConfig {
    [JsonProperty("scheduleOn")]
    public string OtaState { get; set; }
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

public class TemperatureAttribute : DirigeraAttribute
{
    [JsonProperty("currentTemperature")]
    public int CurrentTemperature { get; set; }
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


public class TotalEnergyConsumedAttribute : DirigeraAttribute
{
    [JsonProperty("totalEnergyConsumed")]
    public double TotalEnergyConsumed { get; set; }

    [JsonProperty("totalEnergyConsumedLastUpdated")]
    public DateTime TotalEnergyConsumedLastUpdated { get; set; }
}

public class CurrentPowerUsageAttribute : DirigeraAttribute
{
    [JsonProperty("currentAmps")]
    public double? CurrentAmps { get; set; }

    [JsonProperty("currentVoltage")]
    public double? CurrentVoltage { get; set; }

    [JsonProperty("currentActivePower")]
    public double? CurrentActivePower { get; set; }
}

public class WaterLeakDetectedAttribute : DirigeraAttribute
{
    [JsonProperty("waterLeakDetected")]
    public bool WaterLeakDetected { get; set; }
}

public class OtaStatusAttribute : DirigeraAttribute
{
    [JsonProperty("otaStatus")]
    public string OtaStatus { get; set; }
}

public class OtaProgressAttribute : DirigeraAttribute
{
    [JsonProperty("otaProgress")]
    public int OtaProgress { get; set; }
}

public class IdentifyAttribute : DirigeraAttribute
{
    [JsonProperty("identifyPeriod")]
    public int IdentifyPeriod { get; set; }

    [JsonProperty("identifyStarted")]
    public DateTime IdentifyStarted { get; set; }
}

public class BatteryAttribute : DirigeraAttribute
{
    [JsonProperty("batteryPercentage")]
    public int BatteryPercentage { get; set; }
}




public class UnknownAttribute : DirigeraAttribute
{
    public string MissingPropertyName { get; set; }
    public string Json { get; set; }
}

