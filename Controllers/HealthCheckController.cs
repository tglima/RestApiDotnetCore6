using WebApi.Models.API;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("/")]
    public class HealthCheckController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet("health-check")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Obtém uma mensagem de sucesso")]
        [SwaggerResponse(200, Type = typeof(HealthCheckResponse))]
        public IActionResult Check()
        {
            return Ok(new HealthCheckResponse());
        }
    }
}