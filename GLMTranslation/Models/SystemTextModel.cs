using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GLMTranslation.Models
{
    internal sealed class SystemTextModel
    {
        [JsonPropertyName("role_title")]
        public string Role_title { get; set; } = string.Empty;

        [JsonPropertyName("role")]
        public string Role { get; set; } = string.Empty;

        [JsonPropertyName("rqstd_title")]
        public string Rqstd_title { get; set; } = string.Empty;

        [JsonPropertyName("rqstd")]
        public List<string> Rqstd { get; set; } = [];

        [JsonPropertyName("explain_rqstd")]
        public string Explain_rqstd { get; set; } = string.Empty;

        [JsonPropertyName("input_format_title")]
        public string Input_format_title { get; set; } = string.Empty;

        [JsonPropertyName("input_format")]
        public string Input_format { get; set; } = string.Empty;

        [JsonPropertyName("output_format_title")]
        public string Output_format_title { get; set; } = string.Empty;

        [JsonPropertyName("output_format")]
        public string Output_format { get; set; } = string.Empty;

        [JsonPropertyName("explain_input_format")]
        public string End { get; set; } = string.Empty;
    }
}