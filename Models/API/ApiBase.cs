using System.Text.Json.Serialization;

namespace WebApi.Models.API
{
    public class ApiBase
    {
        /// <summary>
        /// Código identificador único de cada requisição
        /// </summary>
        [JsonPropertyName("code_event")]
        [JsonPropertyOrder(1)]
        public string CodeEvent { get; set; } = string.Empty;
    }
}