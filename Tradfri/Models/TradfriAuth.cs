using System.Text.Json.Serialization;

namespace Tomidix.NetStandard.Tradfri.Models
{
    public class TradfriAuth
    {
        [JsonPropertyName("9091")]
        public string PSK { get; set; }

        [JsonPropertyName("9029")]
        public string FirmwareVersion { get; set; }
    }
}
