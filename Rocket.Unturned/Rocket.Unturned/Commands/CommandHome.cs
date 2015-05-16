using Rocket.API;
using Rocket.Core;
using Rocket.Core.Translations;
using Rocket.Unturned.Player;
using SDG;
using System;
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

        public void Execute(RocketPlayer caller, string[] command)
        {
            Vector3 pos;
            byte rot;
            if (!BarricadeManager.tryGetBed(caller.CSteamID, out pos, out rot))
            {
                RocketChat.Say(caller, RocketTranslationManager.Translate("command_bed_no_bed_found_private"));
            }
            else
            {
                if (caller.Stance == EPlayerStance.DRIVING || caller.Stance == EPlayerStance.SITTING)
                {
                    RocketChat.Say(caller, RocketTranslationManager.Translate("command_generic_teleport_while_driving_error"));
                }
                else
                {
                    caller.Teleport(pos, rot);
                }
            }

        }
    }
}