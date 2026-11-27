using System.Text.Json.Serialization;
using System;
using Tomidix.NetStandard.Tradfri.Extensions;

namespace Tomidix.NetStandard.Tradfri.Models
{
    public class TradfriSmartTask
    {
        [JsonPropertyName("5850")]
        public long LightState { get; set; }

        [JsonPropertyName("9002")]
        [JsonConverter(typeof(MicrosecondEpochConverter))]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("9003")]
        public long ID { get; set; }

        [JsonPropertyName("9040")]
        public TradfriConstAttr TaskType { get; set; }

        [JsonPropertyName("9041")]
        public long RepeatDays { get; set; }

        [JsonPropertyName("9042")]
        public TradfriAction ActionTurnOn { get; set; }

        [JsonPropertyName("9043")]
        public TradfriAction ActionTurnOff { get; set; }

        [JsonPropertyName("9044")]
        public SmartTaskTrigger[] TriggerTimeInterval { get; set; }
    }

    public class SmartTaskTrigger
    {
        [JsonPropertyName("9046")]
        public long StartHour { get; set; }

        [JsonPropertyName("9047")]
        public long StartMin { get; set; }

        [JsonPropertyName("9048")]
        public long EndHour { get; set; }

        [JsonPropertyName("9049")]
        public long EndMin { get; set; }
    }

    public class TradfriAction
    {
        [JsonPropertyName("15013")]
        public TradfriDevice[] Devices { get; set; }

        [JsonPropertyName("5850")]
        public long LightState { get; set; }
    }
}
