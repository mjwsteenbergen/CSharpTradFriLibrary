using System.Buffers.Text;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Text;
using ApiLibs;
using ApiLibs.General;
using ApiLibs.GitHub;
using Newtonsoft.Json;
using Tomidix.NetStandard.Dirigera.Model.Events;

namespace Tomidix.NetStandard.Dirigera.Controller;

public class EventController : SubService<DirigeraController>
{
    public class DirigeraEventArgs : EventArgs
    {
        public DirigeraEventArgs(string message)
        {
            Message = message;
        }

        public DirigeraEventArgs() { }

        public required string Message { get; set; }
        public required DirigeraEvent Event { get; set; }
    }

    public EventController(DirigeraController controller) : base(controller)
    {

    }

    public event EventHandler<DirigeraEventArgs>? OnEventSent;


    // Wrap event invocations inside a protected virtual method
    // to allow derived classes to override the event invocation behavior
    protected virtual void OnRaiseDirigeraEvent(DirigeraEventArgs e)
    {
        // Make a temporary copy of the event to avoid possibility of
        // a race condition if the last subscriber unsubscribes
        // immediately after the null check and before the event is raised.
        EventHandler<DirigeraEventArgs>? raiseEvent = OnEventSent;

        // Event will be null if there are no subscribers
        if (raiseEvent != null)
        {
            // Format the string to send inside the CustomEventArgs parameter
            e.Message += $" at {DateTime.Now}";

            // Call to raise the event.
            raiseEvent(this, e);
        }
    }

    public async Task Connect(CancellationToken cancellationToken)
    {
        ClientWebSocket ws = new();
        ws.Options.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
        ws.Options.SetRequestHeader("Authorization", "Bearer " + Service.token);

        await ws.ConnectAsync(new Uri($"wss://{Service.hostUrl}:8443/v1"), cancellationToken);

        new Task(async () =>
        {

            while (true)
            {
                ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[8192]);
                var result = await ws.ReceiveAsync(buffer, cancellationToken);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    break;
                }

                var message = Encoding.UTF8.GetString(buffer.Array ?? [], 0, result.Count);

                // The message is the json file with ` at TIMESTAMP` text appended for some weird reason
                var split = message.Split(" at ");
                OnRaiseDirigeraEvent(new DirigeraEventArgs
                {
                    Message = message,
                    Event = JsonConvert.DeserializeObject<DirigeraEvent>(split[0])
                });
            }
        }, cancellationToken).Start();


    }

}
