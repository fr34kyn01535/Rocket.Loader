using Rocket.Logging;
using Rocket.RocketAPI;
using SDG;
using System;
using UnityEngine;

namespace Rocket.Commands
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

        public void Execute(RocketPlayer caller, string[] command)
        {
            Vector3 pos;
            byte rot;
            if (!BarricadeManager.tryGetBed(caller.CSteamID, out pos, out rot))
            {
                RocketChatManager.Say(caller, RocketTranslation.Translate("command_bed_no_bed_found_private"));
            }
            else
            {
                if (caller.Stance == EPlayerStance.DRIVING || caller.Stance == EPlayerStance.SITTING)
                {
                    RocketChatManager.Say(caller, RocketTranslation.Translate("command_generic_teleport_while_driving_error"));
                }
                else
                {
                    caller.Teleport(pos, rot);
                }
            }

        }
    }
}