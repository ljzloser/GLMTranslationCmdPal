using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLMTranslation.Model
{
    internal class SystemTextModel
    {
        public string role { get; set; } = string.Empty;
        public string rqstd_title { get; set; } = string.Empty;
        public List<string> rqstd { get; set; } = new();
        public string explain_rqstd { get; set; } = string.Empty;
        public string input_format_title { get; set; } = string.Empty;
        public string input_format { get; set; } = string.Empty;
        public string output_format_title { get; set; } = string.Empty;
        public string output_format { get; set; } = string.Empty;
        public string end { get; set; } = string.Empty;
    }
}
