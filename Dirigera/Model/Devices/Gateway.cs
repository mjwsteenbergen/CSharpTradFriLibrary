using System.Text.Json.Serialization;

namespace Tomidix.NetStandard.Dirigera.Devices;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

public class Gateway : DirigeraDevice
{
    [JsonPropertyName("relationId")]
    public string RelationId { get; set; }

    [JsonPropertyName("deviceSet")]
    public object[] DeviceSet { get; set; }

    [JsonPropertyName("remoteLinks")]
    public object[] RemoteLinks { get; set; }

    [JsonPropertyName("attributes")]
    public GatewayAttributes Attributes { get; set; }

    public override string GetName() => Attributes.CustomName;

    public override string ToString()
    {
        return Attributes.CustomName;
    }
}

public partial class GatewayAttributes : Attributes
{
    [JsonPropertyName("backendConnected")]
    public bool BackendConnected { get; set; }

    [JsonPropertyName("backendConnectionPersistent")]
    public bool BackendConnectionPersistent { get; set; }

    [JsonPropertyName("backendOnboardingComplete")]
    public bool BackendOnboardingComplete { get; set; }

    [JsonPropertyName("backendRegion")]
    public string BackendRegion { get; set; }

    [JsonPropertyName("backendCountryCode")]
    public string BackendCountryCode { get; set; }

    [JsonPropertyName("userConsents")]
    public UserConsent[] UserConsents { get; set; }

    [JsonPropertyName("logLevel")]
    public long LogLevel { get; set; }

    [JsonPropertyName("coredump")]
    public bool Coredump { get; set; }

    [JsonPropertyName("timezone")]
    public string Timezone { get; set; }

    [JsonPropertyName("nextSunSet")]
    public object NextSunSet { get; set; }

    [JsonPropertyName("nextSunRise")]
    public object NextSunRise { get; set; }

    [JsonPropertyName("homestate")]
    public string Homestate { get; set; }

    [JsonPropertyName("countryCode")]
    public string CountryCode { get; set; }

    [JsonPropertyName("isOn")]
    public bool IsOn { get; set; }
}

public partial class UserConsent
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("value")]
    public string Value { get; set; }
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
