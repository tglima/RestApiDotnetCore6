using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;
using WebApi.Helpers;


namespace WebApi.Controllers
{
    [ApiController]
    [Route("/")]
    public class HealthCheckController : ControllerBase
    {
        [AllowAnonymous]
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


