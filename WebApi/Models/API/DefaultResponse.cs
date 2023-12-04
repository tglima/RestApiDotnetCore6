using System.ComponentModel;
using System.Text.Json.Serialization;

namespace WebApi.Models.API
{
    public class DefaultResponse : ApiBase
    {

        public DefaultResponse()
        {
            this.Messages = new();
        }

        public DefaultResponse(string Message, string? CodeEvent)
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