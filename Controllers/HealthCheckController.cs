using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using WebApi.Helpers;


namespace webapi.Controllers
{
    [ApiController]
    [Route("/")]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet("health-check")]
        public IActionResult Check()
        {
            var jsonResponse = new JsonObject
            {
                { "status", Constant.OK },
                { "message", "API IS UP!" }
            };

            return new ObjectResult(jsonResponse) { StatusCode = StatusCodes.Status200OK };
        }
    }
}


