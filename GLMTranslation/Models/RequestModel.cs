using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GLMTranslation.Models
{
    internal sealed class Response_Format
    {
        [JsonPropertyName("@type")]
        public string Type { get; set; } = "json_object";
    }

    internal sealed class MessageModel
    {
        [JsonPropertyName("role")]
        public string Role { get; set; } = "user";

        [JsonPropertyName("content")]
        public string Content { get; set; } = "user";
    }

    internal sealed class ThinkingModel
    {
        [JsonPropertyName("@type")]
        public string Type { get; set; } = "disabled";
    }

    internal sealed class RequestModel
    {
        [JsonPropertyName("model")]
        public string Model { get; set; } = "glm-4.5-flash";

        [JsonPropertyName("messages")]
        public List<MessageModel> Messages { get; set; } = [];

        [JsonPropertyName("temperature")]
        public double Temperature { get; set; } = 0.2;

        [JsonPropertyName("stream")]
        public bool Stream { get; set; }

        [JsonPropertyName("thinking")]
        public ThinkingModel Thinking { get; set; } = new();

        [JsonPropertyName("response_format")]
        public Response_Format Response_format { get; set; } = new();
    }
}