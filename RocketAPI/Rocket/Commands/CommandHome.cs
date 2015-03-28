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

        public void Execute(Steamworks.CSteamID caller, string command)
        {
            Vector3 pos;
            byte rot;
            if (!BarricadeManager.tryGetBed(caller, out pos, out rot))
            {
                RocketChatManager.Say(caller, RocketTranslation.Translate("command_bed_no_bed_found_private"));
            }
            else
            {
                SteamPlayer player = PlayerTool.getSteamPlayer(caller);
                if (player.Player.Stance.Stance == EPlayerStance.DRIVING || player.Player.Stance.Stance == EPlayerStance.SITTING)
                {
                    RocketChatManager.Say(caller, RocketTranslation.Translate("command_generic_teleport_while_driving_error"));
                }
                else
                {
                    player.Player.sendTeleport(pos, rot);
                }
            }

        }
    }
}