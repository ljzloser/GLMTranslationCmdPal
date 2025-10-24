using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLMTranslation.Model
{
    internal class Response_Format
    {
        string type { get; set; } = "json_object";
    }
    internal class MessageModel
    {
        public string role { get; set; } = "user";
        public string content { get; set; } = "user";
    }
    internal class ThinkingModel
    {
        public string type { get; set; } = "disabled";
    }
    internal class RequestModel
    {
        public string model { get; set; } = "glm-4.5-flash";
        public List<MessageModel> messages { get; set; } = [];
        public double temperature { get; set; } = 0.2;
        public bool stream { get; set; }
        public ThinkingModel thinking { get; set; } = new();
        public Response_Format response_format { get; set; } = new();
    }
}
