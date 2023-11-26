using WebApi.Helpers;
using WebApi.Models.API;

namespace WebApi.Models.DTO
{
    public class LogDTO : ApiBase
    {

        public LogDTO()
        {
            this.DtStart = AppHelper.GetDateNow();
        }

        public string ApiKey = string.Empty;
        public string DtStart { get; set; }

        public string DtFinish = string.Empty;

        public string RequestData = string.Empty;

        public string ResponseData = string.Empty;

        public int CodeStatus;
        public List<ReturnDTO> Methods = new();
    }
}