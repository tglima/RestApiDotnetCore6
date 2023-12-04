using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("/")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class SwaggerController : ControllerBase
    {

        [AllowAnonymous]
        [HttpGet("/index.html")]
        public IActionResult RedirectToSwaggerPage()
        {
            return Redirect("/swagger");
        }

        [AllowAnonymous]
        [HttpGet("/swagger.json")]
        public IActionResult RedirectToSwaggerJSON()
        {
            return Redirect("swagger/webapi/swagger.json");
        }
    }
}