using Rocket.Logging;
using Rocket.RocketAPI;
using SDG;
using System;
using UnityEngine;
using System.Linq;

namespace Rocket.Commands
{
    public class CommandInvestigate : IRocketCommand
    {
        public bool RunFromConsole
        {
            get { return true; }
        }

        public string Name
        {
            get { return "investigate"; }
        }

        public string Help
        {
            get { return "Shows you the SteamID64 of a player";}
        }

        public void Execute(RocketPlayer caller, string[] command)
        {
            if (command.Length!=1)
            {
                RocketChatManager.Say(caller, RocketTranslation.Translate("command_generic_invalid_parameter"));
                return;
            }

            SteamPlayer otherPlayer = PlayerTool.getSteamPlayer(command[0]);
            if (otherPlayer != null && otherPlayer.SteamPlayerID.CSteamID.ToString() != caller.ToString())
            {
                RocketChatManager.Say(caller, RocketTranslation.Translate("command_investigate_private", otherPlayer.SteamPlayerID.CharacterName, otherPlayer.SteamPlayerID.CSteamID.ToString()));
            }
            else
            {
                RocketChatManager.Say(caller, RocketTranslation.Translate("command_generic_failed_find_player"));
            }
        }
    }
}