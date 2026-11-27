using System.Text.Json.Serialization;
using System;
using Tomidix.NetStandard.Tradfri.Extensions;

namespace Tomidix.NetStandard.Tradfri.Models
{
    public class TradfriMood
    {
        [JsonPropertyName("15013")]
        public TradfriMoodProperties[] MoodProperties { get; set; }

        [JsonPropertyName("9001")]
        public string Name { get; set; }

        [JsonPropertyName("9002")]
        [JsonConverter(typeof(MicrosecondEpochConverter))]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("9003")]
        public long ID { get; set; }

        [JsonPropertyName("9057")]
        public long The9057 { get; set; }

        [JsonPropertyName("9068")]
        public long The9068 { get; set; }

        public long GroupID { get; set; }
    }

    public class TradfriMoodProperties
    {
        [JsonPropertyName("5706")]
        public string ColorHex { get; set; }

        [JsonPropertyName("5707")]
        public long ColorSaturation { get; set; }

        [JsonPropertyName("5708")]
        public long ColorHue { get; set; }

        [JsonPropertyName("5709")]
        public long ColorX { get; set; }

        [JsonPropertyName("5710")]
        public long ColorY { get; set; }

        [JsonPropertyName("5711")]
        public long Mireds { get; set; }

        [JsonPropertyName("5712")]
        public long TransitionTime { get; set; }

        [JsonPropertyName("5850")]
        public long LightState { get; set; }

        [JsonPropertyName("5851")]
        public long Dimmer { get; set; }

        [JsonPropertyName("9003")]
        public long ID { get; set; }
    }
}
