using Megaphone.Core;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        public string GetStatus()
        {
            return "OK";
        }
    }
}