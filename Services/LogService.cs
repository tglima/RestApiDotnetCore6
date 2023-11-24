using WebApi.Models.DTO;

namespace WebApi.Services
{
    public class LogService
    {
        private LogDTO LogDTO { get; set; }

        public LogService()
        {
            this.LogDTO = new LogDTO();
        }

    }
}