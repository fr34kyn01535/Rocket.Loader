﻿using Rocket.Unturned.Player;
using System.Collections.Generic;

namespace Rocket.Unturned.Commands
{
    public class CommandHeal : IRocketCommand
    {
        public bool RunFromConsole
        {
            get { return true; }
        }

        public string Name
        {
            get { return "heal"; }
        }

        public string Help
        {
            get { return "Heals yourself or somebody else";}
        }

        public string Syntax
        {
            get { return "[player]"; }
        }

        public List<string> Aliases
        {
            get { return new List<string>(); }
        }

        public void Execute(UnturnedPlayer caller, string[] command)
        {
            if (caller != null && command.Length != 1)
            {
                caller.Heal(100);
                caller.Bleeding = false;
                caller.Broken = false;
                caller.Infection = 0;
                caller.Hunger = 0;
                caller.Thirst = 0;
                RocketChat.Say(caller, U.Translate("command_heal_success"));
            }
            else
            {
                UnturnedPlayer otherPlayer = UnturnedPlayer.FromName(command[0]);
                if (otherPlayer != null)
                {
                    otherPlayer.Heal(100);
                    otherPlayer.Bleeding = false;
                    otherPlayer.Broken = false;
                    otherPlayer.Infection = 0;
                    otherPlayer.Hunger = 0;
                    otherPlayer.Thirst = 0;
                    RocketChat.Say(caller, U.Translate("command_heal_success_me", otherPlayer.CharacterName));
                    
                    if(caller != null)
                        RocketChat.Say(otherPlayer, U.Translate("command_heal_success_other", caller.CharacterName));
                }
                else
                {
                    RocketChat.Say(caller, U.Translate("command_generic_target_player_not_found"));
                }
            }
        }
    }
}