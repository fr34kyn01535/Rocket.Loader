﻿using SDG;
using System;
using UnityEngine;
using System.Linq;
using Rocket.API;
using Rocket.Core;
using Rocket.Core.Translations;
using Rocket.Unturned.Player;

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

        public void Execute(RocketPlayer caller, string[] command)
        {
            if (command.Length!=1)
            {
                RocketChat.Say(caller, RocketTranslationManager.Translate("command_generic_invalid_parameter"));
                return;
            }

            SteamPlayer otherPlayer = PlayerTool.getSteamPlayer(command[0]);
            if (otherPlayer != null && (caller == null || otherPlayer.SteamPlayerID.CSteamID.ToString() != caller.ToString()))
            {
                RocketChat.Say(caller, RocketTranslationManager.Translate("command_investigate_private", otherPlayer.SteamPlayerID.CharacterName, otherPlayer.SteamPlayerID.CSteamID.ToString()));
            }
            else
            {
                RocketChat.Say(caller, RocketTranslationManager.Translate("command_generic_failed_find_player"));
            }
        }
    }
}