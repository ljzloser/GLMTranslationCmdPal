using GLMTranslation.Properties;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace GLMTranslation.Commands
{
    internal sealed partial class CopyCommand : CopyTextCommand
    {
        public CopyCommand(string text) : base(text)
        {
            Name = Resources.Copy;
        }
    }
}