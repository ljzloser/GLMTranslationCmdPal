using GLMTranslation.Properties;
using Microsoft.CommandPalette.Extensions.Toolkit;
using System.IO;

namespace GLMTranslation.Helpers
{
    internal sealed class SettingsManager : JsonSettingsManager
    {
        private readonly TextSetting _model = new(
            "Model",
            Resources.Model,
            Resources.Model_Des,
            Resources.Model_Default
            );

        private readonly TextSetting _source_language = new(
            "Source_Language",
            Resources.Source_Language,
            Resources.Source_Language_Des,
            Resources.Source_Language_Default
            );

        private readonly TextSetting _target_language = new(
            "Target_Language",
            Resources.Target_Language,
            Resources.Target_Language_Des,
            Resources.Target_Language_Default
            );

        private readonly TextSetting _target_modes = new(
            "Target_Modes",
            Resources.Target_Modes,
            Resources.Target_Modes_Des,
            Resources.Target_Modes_Default
            );

        private readonly TextSetting _apikey = new(
            "Api_Key",
            Resources.ApiKey,
            Resources.ApiKey_Des,
            string.Empty
            );

        private readonly TextSetting _url = new(
            "Url",
            Resources.Url,
            Resources.Url_Des,
            Resources.Url_Default
            );

        private readonly ToggleSetting _spaceFinish = new(
            "SpaceFinished",
            Resources.SpaceFinish,
            Resources.SpaceFinish_Des,
            Resources.SpaceFinish_Default == "true"
            );

        private readonly ToggleSetting _explain = new(
            "Explain",
            Resources.Explain,
            Resources.Explain_Des,
            true
            );

        public string Model => _model.Value ?? Resources.Url_Default;
        public string TargetLanguage => _target_language.Value ?? Resources.Target_Language_Default;
        public string TargetModes => _target_modes.Value ?? Resources.Url_Default;
        public string ApiKey => _apikey.Value ?? string.Empty;
        public string Url => _url.Value ?? Resources.Url_Default;
        public bool SpaceFinish => _spaceFinish.Value;
        public string SourceLanguage => _source_language.Value ?? Resources.Source_Language_Default;
        public bool Explain => _explain.Value;

        internal static string SettingsPath()
        {
            var dir = Utilities.BaseSettingsPath("Microsoft.CmdPal");
            Directory.CreateDirectory(dir);
            return Path.Combine(dir, "GLMTranslation.json");
        }

        public SettingsManager()
        {
            FilePath = SettingsPath();
            Settings.Add(_url);
            Settings.Add(_apikey);
            Settings.Add(_model);
            Settings.Add(_source_language);
            Settings.Add(_target_language);
            Settings.Add(_target_modes);
            Settings.Add(_spaceFinish);
            Settings.Add(_explain);
            LoadSettings();
            Settings.SettingsChanged += (s, e) => SaveSettings();
        }
    }
}