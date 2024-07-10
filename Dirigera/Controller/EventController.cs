using System.Buffers.Text;
using System.Security.Cryptography;
using System.Text;
using ApiLibs;
using ApiLibs.General;
using ApiLibs.GitHub;
using Newtonsoft.Json;

namespace Tomidix.NetStandard.Dirigera.Controller;

public class EventController : SubService<DirigeraController>
{
    public EventController(DirigeraController controller) : base(controller)
    {
    }

}
