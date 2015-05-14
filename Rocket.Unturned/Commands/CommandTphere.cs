using Rocket.Logging;
using Rocket.RocketAPI;
using SDG;
using System;
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

        public void Execute(RocketPlayer caller, string[] command)
        {
            if (command.Length != 1)
            {
                RocketChatManager.Say(caller, RocketTranslation.Translate("command_generic_invalid_parameter"));
                return;
            }
            RocketPlayer otherPlayer = RocketPlayer.FromName(command[0]);
            if (otherPlayer!=null && otherPlayer != caller)
            {
                otherPlayer.Teleport(caller);
                Logger.Log(RocketTranslation.Translate("command_tphere_teleport_console", otherPlayer.CharacterName, caller.CharacterName));
                RocketChatManager.Say(caller,RocketTranslation.Translate("command_tphere_teleport_from_private",otherPlayer.CharacterName));
                RocketChatManager.Say(otherPlayer, RocketTranslation.Translate("command_tphere_teleport_to_private", caller.CharacterName));
            }
            else
            {
                RocketChatManager.Say(caller, RocketTranslation.Translate("command_generic_failed_find_player"));
            }
        }
    }
}