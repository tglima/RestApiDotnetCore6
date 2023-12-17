using Xunit;
using System.Net;
using WebApi.Models.API;
using System.Net.Http.Json;
using WebApi.Tests.Factories;


namespace WebApi.Tests.Integration.Controllers
{
    public class HealthCheckTests : IClassFixture<WebApiFactory>
    {

        private readonly WebApiFactory _factory;
        private readonly string uri = "health-check";

        public HealthCheckTests(WebApiFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task HealthCheck_ReturnsOkResult()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync(uri);
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }


        [Fact]
        public async Task HealthCheck_ReturnsHealthCheckResponse()
        {
            var client = _factory.CreateClient();
            var response = await client.GetFromJsonAsync<HealthCheckResponse>(uri);
            Assert.NotNull(response);
            Assert.IsType<HealthCheckResponse>(response);
        }
    }
}