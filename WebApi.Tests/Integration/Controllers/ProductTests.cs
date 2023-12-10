using Xunit;
using System.Net;
using WebApi.API;
using WebApi.Helpers;
using WebApi.Tests.Factories;
using System.Net.Http.Json;

namespace WebApi.Tests.Integration.Controllers
{
    public class ProductTests : IClassFixture<WebApiFactory>
    {
        private readonly WebApiFactory _factory;
        private readonly string _uri = "/products/find";

        public ProductTests(WebApiFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetProduct_ReturnsUnauthorized()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync(string.Concat(_uri, "/1"));
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetProducts_ReturnsUnauthorized()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync(_uri);
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetProductWithApiKey_ReturnsUnauthorized()
        {
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add(Constant.API_KEY_HEADER, _factory.InvalidApiKey);
            var response = await client.GetAsync(string.Concat(_uri, "/1"));
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetProductsWithApiKey_ReturnsUnauthorized()
        {
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add(Constant.API_KEY_HEADER, _factory.InvalidApiKey);
            var response = await client.GetAsync(string.Concat(_uri));
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetProductWithApiKey_ReturnsOkResult()
        {
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add(Constant.API_KEY_HEADER, _factory.ApiKeyArray[1]);
            var response = await client.GetAsync(string.Concat(_uri, "/1"));
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetProductsWithApiKey_ReturnsOkResult()
        {
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add(Constant.API_KEY_HEADER, _factory.ApiKeyArray[0]);
            var response = await client.GetAsync(_uri);
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }


        [Fact]
        public async Task GetProductWithApiKey_ReturnProduct()
        {
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add(Constant.API_KEY_HEADER, _factory.ApiKeyArray[1]);
            var response = await client.GetFromJsonAsync<Product>(string.Concat(_uri, "/1"));
            Assert.NotNull(response);
            Assert.IsType<Product>(response);
        }

        [Fact]
        public async Task GetProductsWithApiKey_ReturnArrayProduct()
        {
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add(Constant.API_KEY_HEADER, _factory.ApiKeyArray[1]);
            var response = await client.GetFromJsonAsync<ArrayProduct>(_uri);
            Assert.NotNull(response);
            Assert.IsType<ArrayProduct>(response);
        }


    }
}