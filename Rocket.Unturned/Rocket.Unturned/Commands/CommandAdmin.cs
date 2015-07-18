﻿using Rocket.Unturned.Player;
using System.Collections.Generic;

namespace Rocket.Unturned.Commands
{
    public class CommandAdmin : IRocketCommand
    {
        public bool RunFromConsole
        {
            get { return true; }
        }

        public string Name
        {
            get { return "admin"; }
        }

        public string Help
        {
            get { return "Give a player admin privileges";}
        }

        public string Syntax
        {
            get { return ""; }
        }

        public List<string> Aliases
        {
            get { return new List<string>(); }
        }

        public void Execute(UnturnedPlayer caller, string[] command)
        {
            if (caller == null)
            {
                UnturnedPlayer player = command.GetUnturnedPlayerParameter(0);
                if (player == null)
                {
                    RocketChat.Say(caller, U.Translate("command_generic_invalid_parameter"));
                    return;
                }

                if (!player.IsAdmin)
                {
                    RocketChat.Say(caller, "Successfully admined "+player.CharacterName);
                    player.Admin(true);
                }
            }   
        }
    }
}