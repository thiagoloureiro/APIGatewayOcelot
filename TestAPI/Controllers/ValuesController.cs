using System;
using System.Collections.Generic;
using Megaphone.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ValuesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            Logger.Information("OK");

            return new string[] { DateTime.Now.ToLongDateString(), DateTime.Now.Second.ToString() };
        }
    }
}