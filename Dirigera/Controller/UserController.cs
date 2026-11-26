using System.Buffers.Text;
using System.Security.Cryptography;
using System.Text;
using ApiLibs.JSON;
using ApiLibs.JSON.General;
using ApiLibs.JSON.GitHub;
using System.Text.Json.Serialization;

namespace Tomidix.NetStandard.Dirigera.Controller;

public class UserController : SubService<DirigeraController>
{
    public UserController(DirigeraController controller) : base(controller)
    {
    }

    public Task<List<User>> GetUsers() => MakeRequest(DirigeraJson.Request<List<User>>("users/"));
    public Task<User> GetMe() => MakeRequest(DirigeraJson.Request<User>("users/me/"));

}

public class User
{
    [JsonPropertyName("uid")]
    public required string UID { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("audience")]
    public required string Audience { get; set; }

    [JsonPropertyName("email")]
    public required string Email { get; set; }

    // [JsonPropertyName("createdTimestamp")]
    // public string CreatedTimestamp { get; set; }

    [JsonPropertyName("verifiedUid")]
    public required string VerifiedUid { get; set; }

    [JsonPropertyName("role")]
    public required string Role { get; set; }

}


