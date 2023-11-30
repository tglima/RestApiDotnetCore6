using System.Text;
using WebApi.Helpers;
using System.Text.Json;
using WebApi.Models.DTO;
using System.Text.RegularExpressions;

namespace WebApi.Services
{
    public class LogService
    {
        public LogDTO LogDTO;
        private readonly DbSQLiteContext _dbSQLite;

        public LogService(DbSQLiteContext dbSQLite)
        {
            this.LogDTO = new LogDTO();
            this._dbSQLite = dbSQLite;
        }


        public void SaveLogError(LogErrorDTO logError)
        {
            logError.CodeEvent = this.LogDTO.CodeEvent;
        }

        public async Task SaveLog()
        {
            this.LogDTO.DtFinish = AppHelper.GetDateNow();
            await this._dbSQLite.InsertLog(this.LogDTO);
        }

        public async Task SetRequestData(HttpContext context)
        {
            var headers = context.Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString().Replace("\"", string.Empty));
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


            this.LogDTO.ApiKey = context.Request.Headers[Constant.API_KEY].ToString() ?? string.Empty;

            //Utiliza o Regex.Unescape para converter caracteres unicode em utf8
            this.LogDTO.RequestData = Regex.Unescape(JsonSerializer.Serialize(requestData));
        }

        public void SetResponseData(HttpResponse response, string body)
        {
            var headers = response.Headers.ToDictionary(h => h.Key, h => h.Value.ToString().Replace("\"", string.Empty));

            var statusCode = response.StatusCode;
            var contentType = response.ContentType ?? string.Empty;

            JsonElement? jsonBody = null;

            if (contentType.Contains(Constant.APP_JSON, StringComparison.OrdinalIgnoreCase))
            {
                // Converte a string body para um JsonDocument
                JsonDocument jsonDocument = JsonDocument.Parse(body);

                // Pega o elemento root do jsonDocument
                jsonBody = jsonDocument.RootElement;
            }

            dynamic bodyResponse = jsonBody == null ? body : jsonBody;

            var responseData = new
            {
                headers,
                statusCode,
                contentType,
                body = bodyResponse,
            };

            this.LogDTO.CodeStatus = statusCode;

            //Utiliza o Regex.Unescape para converter caracteres unicode em utf8
            this.LogDTO.ResponseData = Regex.Unescape(JsonSerializer.Serialize(responseData));

        }

    }
}