using System.Text.Json;
using ApiLibs.JSON;

namespace Tomidix.NetStandard.Dirigera;

/// <summary>How the hub's JSON is read: its REST responses and its websocket events.</summary>
public static class DirigeraJson
{
    /// <summary>
    /// System.Text.Json's web defaults: property names are matched case-insensitively and
    /// numbers may arrive as strings. The polymorphic models carry their own converters.
    /// </summary>
    public static JsonSerializerOptions Options { get; } = CreateOptions();

    private static JsonSerializerOptions CreateOptions()
    {
        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        options.MakeReadOnly(populateMissingResolver: true);
        return options;
    }

    /// <summary>A request whose response is read with <see cref="Options"/>.</summary>
    internal static Request<T> Request<T>(string endPoint, Call method = Call.GET, List<Param>? parameters = null) => new(endPoint)
    {
        Method = method,
        Parameters = parameters ?? new List<Param>(),
        Headers = new List<Param>(),
        ParseHandler = Parse<T>
    };

    private static T Parse<T>(RequestResponse response)
    {
        if (typeof(T) == typeof(string))
        {
            return (T)(object)response.Content;
        }

        return JsonSerializer.Deserialize<T>(response.Content, Options)!;
    }
}
