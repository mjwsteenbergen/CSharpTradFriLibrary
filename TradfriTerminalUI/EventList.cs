using System.Collections.Concurrent;
using ApiLibs.MicrosoftGraph;

// using Martijn.Extensions.Linq;
using Spectre.Console;
using Spectre.Console.Json;
using Tomidix.NetStandard.Dirigera.Controller;
using Tomidix.NetStandard.Dirigera.Devices;
using Tomidix.NetStandard.Dirigera.Model.Attributes;

namespace TradfriTerminalUI
{
    public class EventView
    {
        public static async Task Start(EventController controller, List<DirigeraDevice> devices)
        {
            AnsiConsole.Clear();

            var q = new ConcurrentQueue<EventController.DirigeraEventArgs>();

            controller.OnEventSent += (sender, ev) =>
            {
                q.Enqueue(ev);
            };

            await AnsiConsole.Status()
                .StartAsync("Waiting for messages", async ctx =>
                {
                    while (true)
                    {
                        await Task.Delay(500);
                        while (q.Any())
                        {
                            EventController.DirigeraEventArgs? newEvent = null;
                            q.TryDequeue(out newEvent);
                            if (newEvent != null)
                            {

                                var text = MapAttributeToText(newEvent.Message, devices.FirstOrDefault(i => i.Id == newEvent.Event.Data.Id)?.ToString(), newEvent.Event.Data.Attributes);
                                AnsiConsole.MarkupLine($"[dim][[{newEvent.Event.Time}]][/] " + Markup.Escape(text));
                            }
                        }
                    }
                });
        }

        public static string MapAttributeToText(string text, string? deviceName, DirigeraAttribute attribute)
        {
            return attribute switch
            {
                ToggleAttribute toggleAttribute => toggleAttribute.IsOn ? $"{deviceName} was turned on" : $"{deviceName} was turned off",
                LightLevelAttribute toggleAttribute => $"The lightlevel of {deviceName} was changed to {toggleAttribute.LightLevel}",
                RelativeHumidityAttribute relativeHumidity => $"{deviceName} measured a relative humidity of {relativeHumidity.CurrentRelativeHumidity}",
                VOCAttribute vOCAttribute => $"{deviceName} measured a volatile organic compounds of {vOCAttribute.VocIndex}",
                Pm25Attribute pm25Attribute => $"{deviceName} measured a PM25 of {pm25Attribute.CurrentPM25}",
                IlluminanceAttribute illuminance => $"{deviceName} measured luminance of {illuminance.Illuminance}",
                MotionAttribute => $"{deviceName} detected motion",
                UnknownAttribute unknownAttribute => unknownAttribute.Json,
                _ => text
            };
        }

    }
}