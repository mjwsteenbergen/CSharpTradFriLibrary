using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tomidix.NetStandard.Tradfri.Extensions
{
    public class MicrosecondEpochConverter : JsonConverter<DateTime>
    {
        private static readonly DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        // A JSON null becomes DateTime.MinValue rather than an error.
        public override bool HandleNull => true;

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteRawValue((value - _epoch).TotalMilliseconds + "000", skipInputValidation: true);
        }

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
                return DateTime.MinValue;

            long value = reader.GetInt64();
            if (CheckNumberOfDigits(value) == 16)
                return _epoch.AddMilliseconds(value / 1000);
            return _epoch.AddMilliseconds(value * 1000);
        }

        private static int CheckNumberOfDigits(long n)
        {
            return Math.Abs(n).ToString().Length;
        }
    }
}
