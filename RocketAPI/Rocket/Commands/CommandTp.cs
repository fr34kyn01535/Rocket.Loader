using Rocket.Logging;
using Rocket.RocketAPI;
using SDG;
using System;
using UnityEngine;

namespace Rocket.Commands
{
    public class CommandTp : Command
    {
        public CommandTp()
        {
            base.commandName = "tp";
            base.commandHelp = "Teleports you to another player";
            base.commandInfo = base.commandName + " - " + base.commandHelp;
        }

        protected override void execute(SteamPlayerID caller, string command)
        {
            if (!RocketCommand.IsPlayer(caller)) return;

            SteamPlayer otherPlayer;
            if (!String.IsNullOrEmpty(command) && SteamPlayerlist.tryGetSteamPlayer(command, out otherPlayer) && otherPlayer.SteamPlayerID.CSteamID.ToString() != caller.CSteamID.ToString())
            {
                SteamPlayer myPlayer = PlayerTool.getSteamPlayer(caller.CSteamID);

                Vector3 d1 = otherPlayer.Player.transform.position;
                Vector3 vector31 = otherPlayer.Player.transform.rotation.eulerAngles;
                myPlayer.Player.sendTeleport(d1, MeasurementTool.angleToByte(vector31.y));
                Logger.Log(caller.CharacterName + " teleported to " + otherPlayer.SteamPlayerID.CharacterName);
                RocketChatManager.Say(caller.CSteamID, "Teleported to " + otherPlayer.SteamPlayerID.CharacterName);
            }
            else
            {
                RocketChatManager.Say(caller.CSteamID, "Failed to find player");
            }
        }
    }
}