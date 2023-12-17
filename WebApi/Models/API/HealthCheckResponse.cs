using WebApi.Helpers;

namespace WebApi.Models.API
{
    public class HealthCheckResponse
    {
        public string Status { get; set; } = Constant.OK;
        public string Message { get; set; } = "API IS UP!";

    }
}