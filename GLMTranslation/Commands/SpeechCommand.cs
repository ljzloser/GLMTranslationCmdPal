using GLMTranslation.Properties;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using System.Speech.Synthesis;

namespace GLMTranslation.Commands
{
    internal sealed partial class SpeechCommand : InvokableCommand
    {
        public string Text { get; set; }

        public SpeechCommand(string text)
        {
            Name = Resources.Speech;
            Icon = new IconInfo("\ue9f5");
            Text = text;
        }

        public override ICommandResult Invoke()
        {
            SpeechSynthesizer synthesizer = new();
            synthesizer.SetOutputToDefaultAudioDevice();
            synthesizer.SpeakCompleted += (s, e) => { synthesizer.Dispose(); };
            synthesizer.SpeakAsync(Text);
            return CommandResult.KeepOpen();
        }
    }
}