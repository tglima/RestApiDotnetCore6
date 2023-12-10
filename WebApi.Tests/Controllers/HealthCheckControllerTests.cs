using Xunit;
using System;
using System.Linq;
using WebApi.Helpers;
using WebApi.Models.API;
using WebApi.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace WebApi.Tests.Controllers
{
    public class HealthCheckControllerTests
    {
        [Fact]
        public void HealthCheck_ReturnsOkResult()
        {
            var healthCheckController = new HealthCheckController();

            var result = healthCheckController.Check();

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void HealthCheck_ReturnsExpectedJson()
        {

            var healthCheckController = new HealthCheckController();

            var result = healthCheckController.Check() as OkObjectResult;

            Assert.NotNull(result);

            if (result is not null)
            {
                var healthCheckResponse = Assert.IsType<HealthCheckResponse>(result.Value);
                Assert.Equal(Constant.OK, healthCheckResponse.Status);
            }
        }
    }
}