using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using ApiLibs.JSON;

namespace Tomidix.NetStandard.Tradfri.Extensions
{
    /// <summary>How the gateway's JSON is read and written.</summary>
    public static class TradfriJson
    {
        /// <summary>
        /// The gateway uses numeric keys and leaves out what it does not set, so requests leave
        /// out null members too.
        /// </summary>
        public static JsonSerializerOptions Options { get; } = CreateOptions();

        private static JsonSerializerOptions CreateOptions()
        {
            var options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            options.MakeReadOnly(populateMissingResolver: true);
            return options;
        }

        /// <summary>A GET request whose response is read with <see cref="Options"/>.</summary>
        internal static Request<T> Request<T>(string endPoint)
        {
            return Configure(new Request<T>(endPoint));
        }

        /// <summary>Reads the response of <paramref name="request"/> with <see cref="Options"/>.</summary>
        internal static R Configure<R, T>(R request) where R : Request<T>
        {
            request.ParseHandler = response => typeof(T) == typeof(string)
                ? (T)(object)response.Content
                : JsonSerializer.Deserialize<T>(response.Content, Options);
            return request;
        }

        internal static Request<T> Configure<T>(Request<T> request) => Configure<Request<T>, T>(request);

        internal static string Serialize(object value) => JsonSerializer.Serialize(value, value.GetType(), Options);
    }
}
