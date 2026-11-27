using System.Text.Json.Serialization;
using System;
using Tomidix.NetStandard.Tradfri.Extensions;

namespace Tomidix.NetStandard.Tradfri.Models
{
    public class GatewayInfo
    {
        //properties needs naming schema mapping according to https://github.com/ggravlingen/pytradfri/blob/master/pytradfri/const.py
        [JsonPropertyName("9023")]
        public string NTP { get; set; }

        [JsonPropertyName("9029")]
        public string Firmware { get; set; }

        [JsonPropertyName("9054")]
        public long OtaUpdateState { get; set; }

        [JsonPropertyName("9055")]
        public long GatewayUpdateProgress { get; set; }

        [JsonPropertyName("9059")]
        public long CurrentTimeUnix { get; set; }

        [JsonPropertyName("9060")]
        public string CurrentTimeISO8601 { get; set; }

        [JsonPropertyName("9061")]
        public long CommissioningMode { get; set; }

        [JsonPropertyName("9062")]
        public long The9062 { get; set; }

        [JsonPropertyName("9066")]
        public long OtaType { get; set; }

        [JsonPropertyName("9069")]
        [JsonConverter(typeof(MicrosecondEpochConverter))]
        public DateTime FirstSetup { get; set; }

        [JsonPropertyName("9071")]
        public long GatewayTimeSource { get; set; }

        [JsonPropertyName("9072")]
        public long DST_StartMonth { get; set; }

        [JsonPropertyName("9073")]
        public long DST_StartDay { get; set; }

        [JsonPropertyName("9074")]
        public long DST_StartHour { get; set; }

        [JsonPropertyName("9075")]
        public long DST_StartMinute { get; set; }

        [JsonPropertyName("9076")]
        public long DST_EndMonth { get; set; }

        [JsonPropertyName("9077")]
        public long DST_EndDay { get; set; }

        [JsonPropertyName("9078")]
        public long DST_EndHour { get; set; }

        [JsonPropertyName("9079")]
        public long DST_EndMinute { get; set; }

        [JsonPropertyName("9080")]
        public long DST_TimeOffset { get; set; }

        [JsonPropertyName("9081")]
        public string GatewayID { get; set; }

        [JsonPropertyName("9082")]
        public bool The9082 { get; set; }

        [JsonPropertyName("9083")]
        public string HomekitID { get; set; }

        [JsonPropertyName("9092")]
        public long CertificateProv { get; set; }

        [JsonPropertyName("9093")]
        public long AlexaPairStatus { get; set; }

        [JsonPropertyName("9106")]
        public long The9106 { get; set; }
    }
}
