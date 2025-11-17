using GLMTranslation.Commands;
using GLMTranslation.Models;
using GLMTranslation.Properties;
using Microsoft.CommandPalette.Extensions.Toolkit;
using System.Collections.Generic;
using System.Text.Json;
using Windows.System;

namespace GLMTranslation.Helpers
{
    internal static class Instance
    {
        internal static SettingsManager Settings = new();
        internal static readonly SystemTextModel _SystemTextModel = JsonSerializer.Deserialize<SystemTextModel>(Resources.SystemText, ModelContext.Default.SystemTextModel) ?? new();
        internal static readonly string _explain_systemText = string.Empty;
        internal static readonly string _systemText = string.Empty;

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
                _SystemTextModel.Role_title,
                _SystemTextModel.Role,
                _SystemTextModel.Rqstd_title,
                .. _SystemTextModel.Rqstd,
                _SystemTextModel.Explain_rqstd,
                _SystemTextModel.Input_format_title,
                _SystemTextModel.Input_format,
                _SystemTextModel.Output_format_title,
                _SystemTextModel.Output_format,
                _SystemTextModel.End,
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