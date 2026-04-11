using System.Collections.Concurrent;
using ApiLibs.MicrosoftGraph;

// using Martijn.Extensions.Linq;
using Spectre.Console;
using Spectre.Console.Json;
using Tomidix.NetStandard.Dirigera.Controller;
using Tomidix.NetStandard.Dirigera.Devices;
using Tomidix.NetStandard.Dirigera.Model.Events;

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

            controller.SendKeepAliveMessages(CancellationToken.None);

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
                                var getStateChangeText = (DirigeraStateChangedEvent events) =>
                                {
                                    var text = MapAttributeToText(newEvent.Message, devices.FirstOrDefault(i => i.Id == events.Data.Id)?.ToString(), events.Data.Attributes);
                                    return $"[dim][[{newEvent.Event.Time}]][/] " + Markup.Escape(text);
                                };

                                var text = newEvent.Event switch
                                {
                                    DirigeraStateChangedEvent events => getStateChangeText(events),
                                    DirigeraPongEvent pongEvent => $"[dim][[{newEvent.Event.Time}]][/] Replied pong",
                                    UnknownEvent unknownEvent => Markup.Escape(newEvent.Message)
                                };
                                AnsiConsole.MarkupLine(text);
                            }
                        }
                    }
                });
        }

        public static string MapAttributeToText(string text, string? deviceName, EventAttributes attribute)
        {
            return attribute switch
            {
                // ToggleAttribute toggleAttribute => toggleAttribute.IsOn ? $"{deviceName} was turned on" : $"{deviceName} was turned off",
                // LightLevelAttribute toggleAttribute => $"The lightlevel of {deviceName} was changed to {toggleAttribute.LightLevel}",
                // RelativeHumidityAttribute relativeHumidity => $"{deviceName} measured a relative humidity of {relativeHumidity.CurrentRelativeHumidity}",
                // VOCAttribute vOCAttribute => $"{deviceName} measured a volatile organic compounds of {vOCAttribute.VocIndex}",
                // Pm25Attribute pm25Attribute => $"{deviceName} measured a PM25 of {pm25Attribute.CurrentPM25}",
                // IlluminanceAttribute illuminance => $"{deviceName} measured luminance of {illuminance.Illuminance}",
                // CurrentPowerUsageAttribute currentPowerUsageAttribute when currentPowerUsageAttribute.CurrentAmps != null => $"{deviceName} measured ampere of {currentPowerUsageAttribute.CurrentAmps}",
                // CurrentPowerUsageAttribute currentPowerUsageAttribute when currentPowerUsageAttribute.CurrentVoltage != null => $"{deviceName} measured voltage of {currentPowerUsageAttribute.CurrentVoltage}",
                // CurrentPowerUsageAttribute currentPowerUsageAttribute when currentPowerUsageAttribute.CurrentActivePower != null => $"{deviceName} measured power of {currentPowerUsageAttribute.CurrentActivePower}",
                // MotionAttribute => $"{deviceName} detected motion",
                // WaterLeakDetectedAttribute => $"{deviceName} detected something with water",
                // BatteryAttribute battery => $"{deviceName} has a batterypercentage of {battery.BatteryPercentage}%",
                // TemperatureAttribute temperatureAttribute => $"{deviceName} measured a temperature of {temperatureAttribute.CurrentTemperature}",
                // TotalEnergyConsumedAttribute totalEnergy => $"{deviceName} consumed a new total of {totalEnergy.TotalEnergyConsumed} of Power",
                UnknownEventAttributes unknownAttribute => unknownAttribute.Json,
                _ => text
            };
        }

    }
}