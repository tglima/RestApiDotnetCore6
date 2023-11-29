using System.Text.Json.Serialization;

namespace WebApi.Models.API
{
    public class DefRespFail : ApiBase
    {

        public DefRespFail()
        {
            this.Messages = new();
        }

        public DefRespFail(string Message, string? CodeEvent)
        {

            this.CodeEvent = CodeEvent ?? string.Empty;

            this.Messages = new()
            {
                Message
            };
        }


        [JsonPropertyName("messages")]
        [JsonPropertyOrder(int.MaxValue)]
        public List<string> Messages { get; set; }
    }
}