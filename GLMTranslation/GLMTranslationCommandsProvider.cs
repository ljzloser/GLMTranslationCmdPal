// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using GLMTranslation.Helpers;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace GLMTranslation;

public partial class GLMTranslationCommandsProvider : CommandProvider
{
    private readonly ICommandItem[] _commands;

    public GLMTranslationCommandsProvider()
    {
        DisplayName = "GLMTranslation";
        Icon = IconHelpers.FromRelativePath("Assets\\StoreLogo.png");
        Settings = Instance.Settings.Settings;
        _commands = [
            new CommandItem(new GLMTranslationPage()) { Title = DisplayName },
        ];
    }

    public override ICommandItem[] TopLevelCommands()
    {
        return _commands;
    }
}