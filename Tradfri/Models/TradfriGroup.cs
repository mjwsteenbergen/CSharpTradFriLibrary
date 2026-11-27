using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using Tomidix.NetStandard.Tradfri.Extensions;

namespace Tomidix.NetStandard.Tradfri.Models
{
    public class TradfriGroup
    {
        [JsonPropertyName("5850")]
        public long LightState { get; set; }

        [JsonPropertyName("5851")]
        public long LightDimmer { get; set; }

        [JsonPropertyName("9001")]
        public string Name { get; set; }

        [JsonPropertyName("9002")]
        [JsonConverter(typeof(MicrosecondEpochConverter))]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("9003")]
        public long ID { get; set; }

        [JsonPropertyName("9018")]
        public The9018 Devices { get; set; }

        [JsonPropertyName("9039")]
        public long ActiveMood { get; set; }
    }

    public class The9018
    {
        [JsonPropertyName("15002")]
        public The15002 The15002 { get; set; }
    }

    public class The15002
    {
        [JsonPropertyName("9003")]
        public List<long> ID { get; set; }
    }
}
