using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GLMTranslation.Models
{
    [JsonSourceGenerationOptions(WriteIndented = true)]
    [JsonSerializable(typeof(SystemTextModel))]
    [JsonSerializable(typeof(RequestModel))]
    [JsonSerializable(typeof(Dictionary<string, Dictionary<string, string>>))]
    internal sealed partial class ModelContext : JsonSerializerContext
    {
    }
}