using Microsoft.AspNetCore.Authorization;
using WebApi.Models.API;
using WebApi.Models.DTO;
using WebApi.Helpers;
using WebApi.Services;

namespace WebApi.Middlewares
{

    public static class ApiKeyValidationMiddlewareExtensions
    {
        public static IApplicationBuilder UseApiKeyValidationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiKeyValidationMiddleware>();
        }
    }

    public class ApiKeyValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiKeyValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, LogService logService)
        {

            var respFail = new DefRespFail(Constant.MsgStatus401)
            {
                CodeEvent = logService.LogDTO.CodeEvent,
            };

            var returnDTO = new ReturnDTO
            {
                NmMethod = "ValidateApiKey",
                StatusCode = StatusCodeApi.UNNAUTHORIZED,
                Returnbject = respFail
            };


            if (context.GetEndpoint()?.Metadata.GetMetadata<IAllowAnonymous>() == null)
            {

                if (!context.Request.Headers.TryGetValue(Constant.API_KEY, out var extractedApiKey))
                {
                    returnDTO.Info.Add("Api Key was not provided");
                    context.Response.StatusCode = 401;
                    logService.LogDTO.Methods.Add(returnDTO);
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsJsonAsync(respFail);
                    return;
                }

                var apiKeyFromEnv = Environment.GetEnvironmentVariable("API_KEY");
                apiKeyFromEnv = string.IsNullOrEmpty(apiKeyFromEnv) ? string.Empty : apiKeyFromEnv;
                string[] listApiKeyValid = apiKeyFromEnv.Split(';');

                if (!Array.Exists(listApiKeyValid, e => e.Equals(extractedApiKey)))
                {
                    returnDTO.Info.Add("Unauthorized client");
                    context.Response.StatusCode = 401;
                    logService.LogDTO.Methods.Add(returnDTO);
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsJsonAsync(respFail);
                    return;
                }

                logService.LogDTO.ApiKey = extractedApiKey;
            }

            await _next(context);

        }
    }
}