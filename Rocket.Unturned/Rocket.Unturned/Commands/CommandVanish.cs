using Rocket.API;
using Rocket.Core;
using Rocket.Core.Translations;
using Rocket.Unturned.Logging;
using Rocket.Unturned.Player;
using SDG;
using System;

namespace Rocket.Unturned.Commands
{
    public class CommandVanish : IRocketCommand
    {
        public bool RunFromConsole
        {
            get { return false; }
        }

        public string Name
        {
            get { return "vanish"; }
        }

        public string Help
        {
            get { return "Are we rushing in or are we goin' sneaky beaky like?";}
        }

        public void Execute(RocketPlayer caller, string[] command)
        {
            if (caller.Features.VanishMode)
            {
                Logger.Log(RocketTranslationManager.Translate("command_vanish_disable_console", caller.CharacterName));
                RocketChat.Say(caller, RocketTranslationManager.Translate("command_vanish_disable_private"));
                caller.Features.VanishMode = false;
            }
            else
            {
                Logger.Log(RocketTranslationManager.Translate("command_vanish_enable_console", caller.CharacterName));
                RocketChat.Say(caller, RocketTranslationManager.Translate("command_vanish_enable_private"));
                caller.Features.VanishMode = true;
            }
        }
    }
}