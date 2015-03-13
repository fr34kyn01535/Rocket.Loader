using Rocket.Logging;
using Rocket.RocketAPI;
using SDG;
using System;
using UnityEngine;

namespace Rocket.Commands
{
    public class CommandHome : Command
    {
        public CommandHome()
        {
            base.commandName = "home";
            base.commandHelp = "Teleports you to your last bed";
            base.commandInfo = base.commandName + " - " + base.commandHelp;
        }

        protected override void execute(SteamPlayerID caller, string command)
        {
            if (!RocketCommand.IsPlayer(caller)) return;

            Vector3 pos;
            byte rot;
            if (!BarricadeManager.tryGetBed(caller.CSteamID, out pos, out rot))
            {
                RocketChatManager.Say(caller.CSteamID, RocketTranslation.Translate("command_bed_no_bed_found_private"));
            }
            else
            {
                SteamPlayer player = PlayerTool.getSteamPlayer(caller.CSteamID);
                if (player.Player.Stance.Stance == EPlayerStance.DRIVING || player.Player.Stance.Stance == EPlayerStance.SITTING)
                {
                    RocketChatManager.Say(caller.CSteamID, RocketTranslation.Translate("command_generic_teleport_while_driving_error"));
                }
                else
                {
                    player.Player.sendTeleport(pos, rot);
                }
            }

        }
    }
}