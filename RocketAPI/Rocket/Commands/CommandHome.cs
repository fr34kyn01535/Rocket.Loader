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
                RocketChatManager.Say(caller.CSteamID, "You do not have a bed to teleport to.");
            }
            else
            {
                SteamPlayer player = PlayerTool.getSteamPlayer(caller.CSteamID);
                if (player.Player.Stance.Stance == EPlayerStance.DRIVING || player.Player.Stance.Stance == EPlayerStance.SITTING)
                {
                    RocketChatManager.Say(caller.CSteamID, "You cannot teleport home when driving or riding in a vehicle.");
                }
                else
                {
                    player.Player.sendTeleport(pos, rot);
                }
            }

        }
    }
}