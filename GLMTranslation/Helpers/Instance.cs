using GLMTranslation.Commands;
using GLMTranslation.Model;
using GLMTranslation.Properties;
using Microsoft.CommandPalette.Extensions.Toolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.System;

namespace GLMTranslation.Helpers
{
    internal static class Instance
    {
        internal static SettingsManager Settings = new();
        internal readonly static SystemTextModel _SystemTextModel = JsonSerializer.Deserialize<SystemTextModel>(Resources.SystemText)??new();
        internal readonly static string _explain_systemText = string.Empty;
        internal readonly static string _systemText = string.Empty;
        internal static string SystemText
        {
            get
            {
                if (Settings.Explain)
                    return _explain_systemText;
                return _systemText;
            }
        }
        static Instance()
        {
            List<string> strings =
            [
                _SystemTextModel.role,
                _SystemTextModel.rqstd_title,
                .. _SystemTextModel.rqstd,
                _SystemTextModel.explain_rqstd,
                _SystemTextModel.input_format_title,
                _SystemTextModel.input_format,
                _SystemTextModel.output_format_title,
                _SystemTextModel.output_format,
                _SystemTextModel.end,
            ];
            _explain_systemText = string.Join("\n", strings);
            strings.RemoveAt(strings.Count - 6);
            _systemText = string.Join("\n", strings);
        }
        internal static CommandContextItem[] LoadCommands(string text)
        {
            return [
                new CommandContextItem(new SpeechCommand(text))
                {
                    RequestedShortcut = KeyChordHelpers.FromModifiers(true, false, false, false, (int)VirtualKey.Enter, 0),
                },
            ];
        }
    }
}
