﻿using Rocket.Logging;
using Rocket.RocketAPI;
using SDG;
using System;

namespace Rocket.Commands
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

        public void Execute(RocketPlayer caller, string command)
        {
            if (caller.Features.VanishMode)
            {
                Logger.Log(RocketTranslation.Translate("command_vanish_disable_console", caller.CharacterName));
                RocketChatManager.Say(caller, RocketTranslation.Translate("command_vanish_disable_private"));
                caller.Features.VanishMode = false;
            }
            else
            {
                Logger.Log(RocketTranslation.Translate("command_vanish_enable_console", caller.CharacterName));
                RocketChatManager.Say(caller, RocketTranslation.Translate("command_vanish_enable_private"));
                caller.Features.VanishMode = true;
            }
        }
    }
}