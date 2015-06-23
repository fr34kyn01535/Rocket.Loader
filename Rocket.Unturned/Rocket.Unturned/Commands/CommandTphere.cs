using Rocket.API;
using Rocket.Core;
using Rocket.Core.Translations;
using Rocket.Unturned.Logging;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rocket.Unturned.Commands
{
    internal class CommandTphere : IRocketCommand
    {
        public bool RunFromConsole
        {
            get { return false; }
        }

        public string Name
        {
            get { return "tphere"; }
        }

        public string Help
        {
            get { return "Teleports another player to you";}
        }

        public string Syntax
        {
            get { return "<player>"; }
        }

        public List<string> Aliases
        {
            get { return new List<string>(); }
        }

        public void Execute(RocketPlayer caller, string[] command)
        {
            if (command.Length != 1)
            {
                RocketChat.Say(caller, RocketTranslationManager.Translate("command_generic_invalid_parameter"));
                return;
            }
            RocketPlayer otherPlayer = RocketPlayer.FromName(command[0]);
            if (otherPlayer!=null && otherPlayer != caller)
            {
                otherPlayer.Teleport(caller);
                Logger.Log(RocketTranslationManager.Translate("command_tphere_teleport_console", otherPlayer.CharacterName, caller.CharacterName));
                RocketChat.Say(caller, RocketTranslationManager.Translate("command_tphere_teleport_from_private", otherPlayer.CharacterName));
                RocketChat.Say(otherPlayer, RocketTranslationManager.Translate("command_tphere_teleport_to_private", caller.CharacterName));
            }
            else
            {
                RocketChat.Say(caller, RocketTranslationManager.Translate("command_generic_failed_find_player"));
            }
        }
    }
}