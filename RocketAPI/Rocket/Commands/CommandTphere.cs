using Rocket.Logging;
using Rocket.RocketAPI;
using SDG;
using System;
using UnityEngine;

namespace Rocket.Commands
{
    internal class CommandTphere : Command
    {
        public CommandTphere()
        {
            base.commandName = "tphere";
            base.commandHelp = "Teleports another player to you";
            base.commandInfo = base.commandName + " - " + base.commandHelp;
        }

        protected override void execute(SteamPlayerID caller, string command)
        {
            SteamPlayer otherPlayer;
            if (!String.IsNullOrEmpty(command) && SteamPlayerlist.tryGetSteamPlayer(command, out otherPlayer) && otherPlayer.SteamPlayerID.CSteamID.ToString() != caller.CSteamID.ToString())
            {
                SteamPlayer myPlayer = PlayerTool.getSteamPlayer(caller.CSteamID);

                Vector3 d1 = myPlayer.Player.transform.position;
                Vector3 vector31 = myPlayer.Player.transform.rotation.eulerAngles;
                otherPlayer.Player.sendTeleport(d1, MeasurementTool.angleToByte(vector31.y));
                Logger.Log(otherPlayer.SteamPlayerID.CharacterName + " was teleported to " + myPlayer.SteamPlayerID.CharacterName);
                RocketChatManager.Say(caller.CSteamID, "Teleported " + otherPlayer.SteamPlayerID.CharacterName + " to you");
                RocketChatManager.Say(otherPlayer.SteamPlayerID.CSteamID, "You were teleported to " + myPlayer.SteamPlayerID.CharacterName);
            }
            else
            {
                RocketChatManager.Say(caller.CSteamID, "Failed to find player");
            }
        }
    }
}