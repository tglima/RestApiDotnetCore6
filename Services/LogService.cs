using System.Text;
using WebApi.Helpers;
using System.Text.Json;
using WebApi.Models.DTO;

namespace WebApi.Services
{
    public class LogService
    {
        public LogDTO LogDTO;

        public LogService()
        {
            this.LogDTO = new LogDTO();
        }


        public void SaveLogError(LogErrorDTO logError)
        {
            logError.CodeEvent = this.LogDTO.CodeEvent;
        }

        public void SaveLog()
        {
            this.LogDTO.DtFinish = AppHelper.GetDateNow();

        }

        public async Task SetRequestData(HttpContext context)
        {
            var headers = context.Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString());
            var method = context.Request.Method;
            var query = context.Request.Query.ToDictionary(q => q.Key, q => q.Value.ToString());
            var url = string.Concat(context.Request.Path, context.Request.QueryString);

            using var reader = new StreamReader(context.Request.Body, Encoding.UTF8);
            var body = await reader.ReadToEndAsync();

            var requestData = new
            {
                headers,
                body,
                method,
                query,
                url
            };

            this.LogDTO.RequestData = JsonSerializer.Serialize(requestData);
            this.LogDTO.ApiKey = context.Request.Headers[Constant.API_KEY].ToString() ?? string.Empty;
        }

        public void SetResponseData(HttpResponse response, string body)
        {
            var headers = response.Headers.ToDictionary(h => h.Key, h => h.Value.ToString());
            var statusCode = response.StatusCode;
            var contentType = response.ContentType ?? string.Empty;

            var responseData = new
            {
                headers,
                statusCode,
                contentType,
                body,
            };

            this.LogDTO.ResponseData = JsonSerializer.Serialize(responseData);

        }

    }
}