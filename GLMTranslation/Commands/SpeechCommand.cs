using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;
using GLMTranslation.Properties;

namespace GLMTranslation.Commands
{
    internal partial class SpeechCommand: InvokableCommand
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
            SpeechSynthesizer synthesizer = new SpeechSynthesizer();
            synthesizer.SetOutputToDefaultAudioDevice();
            synthesizer.SpeakCompleted += (s, e) => { synthesizer.Dispose(); };
            synthesizer.SpeakAsync(Text);
            return CommandResult.KeepOpen();
        }
    }
}
