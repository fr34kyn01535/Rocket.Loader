using Rocket.API;
using Rocket.Core;
using Rocket.Core.Translations;
using Rocket.Unturned.Logging;
using Rocket.Unturned.Player;
using SDG;
using System;
using System.Collections.Generic;

namespace Rocket.Unturned.Commands
{
    public class CommandGod : IRocketCommand
    {
        public bool RunFromConsole
        {
            get { return false; }
        }

        public string Name
        {
            get { return "god"; }
        }

        public string Help
        {
            get { return "Cause you ain't givin a shit";}
        }

        public string Syntax
        {
            get { return ""; }
        }

        public List<string> Aliases
        {
            get { return new List<string>(); }
        }

        public void Execute(RocketPlayer caller, string[] command)
        {
            if (caller.Features.GodMode)
            {
                Logger.Log(RocketTranslationManager.Translate("command_god_disable_console", caller.CharacterName));
                RocketChat.Say(caller, RocketTranslationManager.Translate("command_god_disable_private"));
                caller.Features.GodMode = false;
            }
            else
            {
                Logger.Log(RocketTranslationManager.Translate("command_god_enable_console", caller.CharacterName));
                RocketChat.Say(caller, RocketTranslationManager.Translate("command_god_enable_private"));
                caller.Features.GodMode = true;
            }
        }
    }
}