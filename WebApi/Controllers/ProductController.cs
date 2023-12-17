using WebApi.API;
using WebApi.Services;
using WebApi.Models.API;
using WebApi.Models.DTO;
using WebApi.Models.Examples;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductsController : ControllerBase
    {

        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            this._productService = productService;
        }

        // GET: /products/find
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Rota para obter todos os produtos disponíveis")]
        [SwaggerResponse(200, Type = typeof(ArrayProduct))]
        [SwaggerResponse(400, Type = typeof(DefaultResponse))]
        [SwaggerResponse(401, Type = typeof(DefaultResponse))]
        [SwaggerResponse(500, Type = typeof(DefaultResponse))]
        [SwaggerResponseExample(200, typeof(ArrayProductExample))]
        [SwaggerResponseExample(400, typeof(InvalidRequestExample))]
        [SwaggerResponseExample(401, typeof(AuthFailedExample))]
        [SwaggerResponseExample(500, typeof(ServerErrorExample))]
        [HttpGet("find")]
        public IActionResult GetProducts()
        {
            return ReturnDTO.ToActionResult(this._productService.FindProducts());
        }

        // GET: /products/find/{id}
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Rota para obter o produto de acordo com o seu id")]
        [SwaggerResponse(200, Type = typeof(Product))]
        [SwaggerResponse(400, Type = typeof(DefaultResponse))]
        [SwaggerResponse(401, Type = typeof(DefaultResponse))]
        [SwaggerResponse(500, Type = typeof(DefaultResponse))]
        [SwaggerResponseExample(200, typeof(ProductExample))]
        [SwaggerResponseExample(400, typeof(InvalidRequestExample))]
        [SwaggerResponseExample(401, typeof(AuthFailedExample))]
        [SwaggerResponseExample(500, typeof(ServerErrorExample))]
        [HttpGet("find/{id}")]
        public IActionResult GetProductById(string id)
        {
            return ReturnDTO.ToActionResult(this._productService.FindProductById(id));
        }

    }
}