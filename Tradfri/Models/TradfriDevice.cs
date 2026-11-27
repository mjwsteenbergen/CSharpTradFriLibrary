using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using Tomidix.NetStandard.Tradfri.Extensions;

namespace Tomidix.NetStandard.Tradfri.Models
{
    public enum DeviceType
    {
        Remote = 0,
        Unknown_1 = 1,
        Light = 2,
        ControlOutlet = 3,
        MotionSensor = 4,
        Unknown_3 = 5,
        Unknown_4 = 6,
        Blind = 7,
        Unknown_6 = 8,
        Unknown_7 = 9,
        Unknown_8 = 10,
        Unknown_9 = 11
    }

    public enum PowerSource
    {
        DCPower = 0,
        InternalBattery = 1,
        ExternalBattery = 2,
        Battery = 3, //used by motion sensor
        POE = 4, //power over ethernet
        USB = 5,
        ACPower = 6,
        Solar = 7,
        Unknown_1 = 8,
        Unknown_2 = 9,
    }

    public class TradfriDevice
    {
        [JsonPropertyName("15009")]
        public List<RootSwitch> RootSwitch { get; set; }

        [JsonPropertyName("3")]
        public DeviceInfo Info { get; set; }

        //0 - remote, 2 - light
        [JsonPropertyName("5750")]
        public DeviceType DeviceType { get; set; }

        [JsonPropertyName("9001")]
        public string Name { get; set; }

        [JsonPropertyName("9002")]
        [JsonConverter(typeof(MicrosecondEpochConverter))]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("9003")]
        public long ID { get; set; }

        [JsonPropertyName("9019")]
        public long ReachableState { get; set; }

        [JsonPropertyName("9020")]
        [JsonConverter(typeof(MicrosecondEpochConverter))]
        public DateTime LastSeen { get; set; }

        [JsonPropertyName("9054")]
        public long OtaUpdateState { get; set; }

        [JsonPropertyName("3311")]
        public List<LightControl> LightControl { get; set; }

        [JsonPropertyName("3312")]
        public List<Control> Control { get; set; }

        [JsonPropertyName("15015")]
        public List<Blind> Blind { get; set; }
    }

    public class DeviceInfo
    {
        [JsonPropertyName("0")]
        public string Manufacturer { get; set; }

        [JsonPropertyName("1")]
        public string DeviceType { get; set; }

        [JsonPropertyName("2")]
        public string Serial { get; set; }

        [JsonPropertyName("3")]
        public string FirmwareVersion { get; set; }

        [JsonPropertyName("6")]
        public PowerSource PowerSource { get; set; }

        [JsonPropertyName("9")]
        public long Battery { get; set; }
    }

    public class RootSwitch
    {
        [JsonPropertyName("9003")]
        public long RootSwitchID { get; set; }
    }

    public class Control
    {
        [JsonPropertyName("5850")]
        public Bool? State { get; set; }

        [JsonPropertyName("5851")]
        public long Dimmer { get; set; }

        [JsonPropertyName("9003")]
        public long ID { get; set; }
    }

    public class LightControl : Control
    {
        [JsonPropertyName("5706")]
        public string ColorHex { get; set; }

        [JsonPropertyName("5707")]
        public long ColorHue { get; set; }

        [JsonPropertyName("5708")]
        public long ColorSaturation { get; set; }

        [JsonPropertyName("5709")]
        public long ColorX { get; set; }

        [JsonPropertyName("5710")]
        public long ColorY { get; set; }

        [JsonPropertyName("5711")]
        public long Mireds { get; set; }
    }

    public class Blind
    {
        [JsonPropertyName("9003")]
        public long ID { get; set; }

        [JsonPropertyName("5536")]
        public long Position { get; set; }
    }

    public class DeviceList
    {
        [JsonPropertyName("9003")]
        public List<long> DevicesIds { get; set; }
    }
}
