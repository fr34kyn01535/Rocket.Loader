using SDG.Unturned;
using System;
using UnityEngine;
using System.Linq;
using Rocket.API;
using Rocket.Core;
using Rocket.Unturned.Player;
using System.Collections.Generic;

namespace Rocket.Unturned.Commands
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

        public string Syntax
        {
            get { return "<player>"; }
        }

        public List<string> Aliases
        {
            get { return new List<string>(); }
        }

        public void Execute(UnturnedPlayer caller, string[] command)
        {
            if (command.Length!=1)
            {
                RocketChat.Say(caller, U.Translate("command_generic_invalid_parameter"));
                return;
            }

            SteamPlayer otherPlayer = PlayerTool.getSteamPlayer(command[0]);
            if (otherPlayer != null && (caller == null || otherPlayer.SteamPlayerID.CSteamID.ToString() != caller.ToString()))
            {
                RocketChat.Say(caller, U.Translate("command_investigate_private", otherPlayer.SteamPlayerID.CharacterName, otherPlayer.SteamPlayerID.CSteamID.ToString()));
            }
            else
            {
                RocketChat.Say(caller, U.Translate("command_generic_failed_find_player"));
            }
        }
    }
}