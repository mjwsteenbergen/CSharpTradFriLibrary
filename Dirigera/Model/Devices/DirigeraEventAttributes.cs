using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Tomidix.NetStandard.Dirigera.Devices;

public class DirigeraEventAttributesDataConverter : JsonConverter<EventAttributes>

{
    public override EventAttributes ReadJson(JsonReader reader, Type objectType, [AllowNull] EventAttributes existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JToken jObject = JToken.ReadFrom(reader);

        string deviceType = "";

        try
        {
            if (jObject.Type is not JTokenType.None and not JTokenType.Null)
            {
                deviceType = jObject["deviceType"]?.ToObject<string>() ?? "";
            }
        }
        catch { }

        EventAttributes result = deviceType switch
        {
            "environmentSensor" => new EnvironmentSensorEventAttributes(),
            // "gateway" => new Gateway(),
            "light" => new LightEventAttributes(),
            "motionSensor" => new MotionSensorEventAttributes(),
            "lightSensor" => new LightSensorEventAttributes(),
            "outlet" => new OutletEventAttributes(),
            "waterSensor" => new WaterSensorEventAttributes(),
            _ => new UnknownEventAttributes
            {
                DeviceType = deviceType,
                Json = jObject.ToString(Formatting.Indented)
            }
        };


        serializer.Populate(jObject.CreateReader(), result);
        return result;
    }

    public override bool CanWrite => false;

    public override void WriteJson(JsonWriter writer, [AllowNull] EventAttributes value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}

public class UnknownEventAttributes : EventAttributes
{
    public required string DeviceType { get; set; }
    public required string Json { get; set; }
}