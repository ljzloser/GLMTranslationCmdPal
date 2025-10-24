using GLMTranslation.Properties;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLMTranslation.Commands
{
    internal partial class CopyCommand : CopyTextCommand
    {
        public CopyCommand(string text) : base(text)
        {
            Name = Resources.Copy;
        }
    }
}