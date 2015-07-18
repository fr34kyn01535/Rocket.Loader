using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Collections.Generic;
using UnityEngine;

namespace Rocket.Unturned.Commands
{
    public class CommandHome : IRocketCommand
    {
        public bool RunFromConsole
        {
            get { return false; }
        }

        public string Name
        {
            get { return "home"; }
        }

        public string Help
        {
            get { return "Teleports you to your last bed";}
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
            Vector3 pos;
            byte rot;
            if (!BarricadeManager.tryGetBed(caller.CSteamID, out pos, out rot))
            {
                RocketChat.Say(caller, U.Translate("command_bed_no_bed_found_private"));
            }
            else
            {
                if (caller.Stance == EPlayerStance.DRIVING || caller.Stance == EPlayerStance.SITTING)
                {
                    RocketChat.Say(caller, U.Translate("command_generic_teleport_while_driving_error"));
                }
                else
                {
                    caller.Teleport(pos, rot);
                }
            }

        }
    }
}