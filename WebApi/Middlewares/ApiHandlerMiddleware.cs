using WebApi.Helpers;
using WebApi.Services;
using WebApi.Models.API;
using WebApi.Models.DTO;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Middlewares
{

    public static class ApiHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseApiHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiHandlerMiddleware>();
        }
    }

    public class ApiHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        public async Task InvokeAsync(HttpContext context, LogService logService)
        {
            await logService.SetRequestData(context);

            var respValidateApiKey = ValidateApiKey(context, logService.LogDTO);

            logService.LogDTO.Methods.Add(respValidateApiKey);

            string responseBody = string.Empty;

            if (respValidateApiKey.StatusCode.Equals(StatusCodeApi.UNNAUTHORIZED))
            {
                context.Response.StatusCode = (int)respValidateApiKey.StatusCode;
                context.Response.ContentType = Constant.APP_JSON;
                responseBody = AppHelper.ToJSON(respValidateApiKey.Returnbject);
                await context.Response.WriteAsJsonAsync(respValidateApiKey.Returnbject);
            }
            else
            {


                try
                {

                    var response = context.Response;
                    var originalBody = response.Body;

#pragma warning disable IDE0063 // Usar a instrução 'using' simples
                    using (var newBody = new MemoryStream())
                    {
                        response.Body = newBody;

                        await _next(context);

                        // A partir deste ponto, seria após a execução do controller
                        newBody.Seek(0, SeekOrigin.Begin);
                        responseBody = await new StreamReader(newBody).ReadToEndAsync();

                        // Restaurar o corpo original da resposta
                        newBody.Seek(0, SeekOrigin.Begin);
                        await newBody.CopyToAsync(originalBody);
                        response.Body = originalBody;
                    }
#pragma warning restore IDE0063 // Usar a instrução 'using' simples
                }
                catch (Exception ex)
                {
                    await logService.SaveLogError(new LogErrorDTO(ex, "Middleware.InvokeAsync"));
                    var response = context.Response;
                    response.ContentType = Constant.APP_JSON; ;
                    var respError = new DefaultResponse(Constant.MsgStatus500, logService.LogDTO.CodeEvent);
                    response.StatusCode = (int)StatusCodeApi.ERROR;
                    context.Response.ContentType = Constant.APP_JSON;
                    responseBody = AppHelper.ToJSON(respError);

                    await context.Response.WriteAsJsonAsync(respError);
                }
            }

            logService.SetResponseData(context.Response, responseBody);
            await logService.SaveLog();
        }


        private static ReturnDTO ValidateApiKey(HttpContext context, LogDTO logDTO)
        {

            bool isValidApiKey = true;

            var returnDTO = new ReturnDTO
            {
                NmMethod = "ValidateApiKey"
            };

            if (context.GetEndpoint()?.Metadata.GetMetadata<IAllowAnonymous>() == null)
            {

                var apiKeyFromEnv = Environment.GetEnvironmentVariable("API_KEY") ?? string.Empty;
                string[] listApiKeyValid = apiKeyFromEnv.Split(';');

                if (string.IsNullOrEmpty(logDTO.ApiKey))
                {
                    returnDTO.Info.Add("Api Key was not provided");
                    isValidApiKey = false;
                }


                if (!string.IsNullOrEmpty(logDTO.ApiKey) && !Array.Exists(listApiKeyValid, apikey => logDTO.ApiKey.Equals(apikey)))
                {
                    returnDTO.Info.Add("Unauthorized key");
                    isValidApiKey = false;
                }
            }


            returnDTO.StatusCode = isValidApiKey ? StatusCodeApi.SUCCESS : StatusCodeApi.UNNAUTHORIZED;
            returnDTO.Returnbject = isValidApiKey ? null : new DefaultResponse(Constant.MsgStatus401, logDTO.CodeEvent);

            return returnDTO;
        }
    }
}