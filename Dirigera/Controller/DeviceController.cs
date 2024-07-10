using ApiLibs.General;
using Tomidix.NetStandard.Dirigera.Devices;
using Newtonsoft.Json;
using ApiLibs;
using Tomidix.NetStandard.Dirigera.Model.Attributes;
using Martijn.Extensions.Linq;

namespace Tomidix.NetStandard.Dirigera.Controller;

public class DeviceController : SubService<DirigeraController>
{
    private Service service;
    public DeviceController(DirigeraController controller) : base(controller)
    {
        service = controller;
    }

    public Task<List<DirigeraDevice>> GetDevices() => MakeRequest<List<DirigeraDevice>>("devices/").ContinueWith(task =>
    {
        task.Result.OfType<LightSensor>().Foreach(lightsensor =>
        {
            var motionSensorMatch = task.Result
                .OfType<MotionSensor>()
                .FirstOrDefault(motionsensor => motionsensor.Attributes.SerialNumber.Equals(lightsensor.Attributes.SerialNumber));

            if (motionSensorMatch != null)
            {
                lightsensor.Attributes.CustomName = motionSensorMatch.Attributes.CustomName;
            }
        });

        task.Result.Foreach(i => i.Search(Service));

        return task.Result;
    });
    public Task<string> GetDevicesJson() => MakeRequest<string>("devices/");

    public Task<string> ChangeAttributes<T>(string deviceId, PostingAttributes<T> attributes) where T : DirigeraAttribute => MakeRequest<string>("devices/" + deviceId, Call.PATCH, content: new object[] { attributes }, statusCode: System.Net.HttpStatusCode.Accepted);

    public Task<string> Toggle(Light l) => Toggle(l.Id, !l.Attributes.IsOn).ContinueWith((a) =>
    {
        l.Attributes.IsOn = !l.Attributes.IsOn;
        return a.Result;
    });

    public Task<string> Toggle(string id, bool isOn) => ChangeAttributes(id, new PostingAttributes<ToggleAttribute>(new ToggleAttribute
    {
        IsOn = isOn
    }));

    public Task<string> SetLightLevel(Light l, int level) => SetLightLevel(l.Id, level).ContinueWith((a) =>
        {
            l.Attributes.LightLevel = level;
            return a.Result;
        });

    public Task<string> SetLightLevel(string id, int level) => ChangeAttributes(id, new PostingAttributes<LightLevelAttribute>(new LightLevelAttribute
    {
        LightLevel = level
    }));


    public Task<string> SetColorTemperature(Light l, int temperature) => Toggle(l.Id, !l.Attributes.IsOn).ContinueWith((a) =>
        {
            l.Attributes.ColorTemperature = temperature;
            return a.Result;
        });

    public Task<string> SetLightTemperature(string id, int temperature) => ChangeAttributes(id, new PostingAttributes<ColorTemperatureAttribute>(new ColorTemperatureAttribute
    {
        ColorTemperature = temperature
    }));

    public Task SetMotionDetectedDelay(MotionSensor motionSensor, int delay) => SetMotionDetectedDelay(motionSensor.Id, delay).ContinueWith(result =>
    {
        motionSensor.Attributes.MotionDetectedDelay = delay;
    });

    public Task SetMotionDetectedDelay(string id, int delay) => ChangeAttributes(id, new PostingAttributes<MotionDetectedDelayAttribute>(new MotionDetectedDelayAttribute
    {
        MotionDetectedDelay = delay
    }));
}

public class PostingAttributes<T> where T : DirigeraAttribute
{
    public PostingAttributes(T properties, int? transitionTime = null)
    {
        TransitionTime = transitionTime;
        Attributes = properties;
    }

    [JsonProperty("attributes")]
    public T Attributes { get; set; }


    [JsonProperty("transitionTime")]
    public int? TransitionTime { get; set; }
}


