using System.Net;
using ApiLibs.JSON;

namespace SerializationTest;

/// <summary>Answers every request with a fixed body, so responses go through ApiLibs.JSON's MakeRequest as in the real controllers.</summary>
public class FakeService : Service
{
    private sealed class Canned : ICallImplementation
    {
        public string Body = "";

        public override Task<RequestResponse> ExecuteRequest(Service service, Request request) =>
            Task.FromResult(new RequestResponse(HttpStatusCode.OK, "OK", "", "", Body, null, request, service));
    }

    private FakeService(Canned canned) : base(canned)
    {
    }

    public static Task<T> Read<T>(string body) => new FakeService(new Canned { Body = body }).MakeRequest<T>("x");
}
