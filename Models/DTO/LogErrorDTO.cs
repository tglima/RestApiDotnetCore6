using WebApi.Helpers;
using WebApi.Models.API;

namespace WebApi.Models.DTO
{
    public class LogErrorDTO : ApiBase
    {

        public LogErrorDTO()
        {
            this.DtRegister = AppHelper.GetDateNow();
        }

        public string DtRegister { get; set; }
        public string Method = string.Empty;
        public string ExceptionMessage = string.Empty;
        public string StackTrace = string.Empty;

    }
}