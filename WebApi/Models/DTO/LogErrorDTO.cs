using WebApi.Helpers;
using WebApi.Models.API;

namespace WebApi.Models.DTO
{
    public class LogErrorDTO : ApiBase
    {
        public LogErrorDTO() { }

        public LogErrorDTO(Exception exception, string method)
        {
            this.Method = method;
            this.ExceptionMessage = exception.Message;
            this.StackTrace = exception.StackTrace ?? string.Empty;
        }

        public string DtRegister = AppHelper.GetDateNow();
        public string Method = string.Empty;
        public string ExceptionMessage = string.Empty;
        public string StackTrace = string.Empty;

    }
}