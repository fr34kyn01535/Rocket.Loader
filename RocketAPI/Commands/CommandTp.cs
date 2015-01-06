﻿using SDG;
using UnityEngine;

namespace Rocket
{
    public class CommandTp : Command
    {
        public CommandTp() {
            base.commandName = "tp";
            base.commandInfo = base.commandHelp = "Teleports you to another player";
        }

        protected override void execute(SteamPlayerID caller, string command)
        {
            if (command.Length < commandName.Length + 2) return;
            SteamPlayer otherPlayer;
            if (SteamPlayerlist.tryGetSteamPlayer(command.Substring(commandName.Length + 2), out otherPlayer) && otherPlayer.SteamPlayerID.CSteamID.ToString() != caller.CSteamID.ToString())
            {
                SteamPlayer myPlayer = PlayerTool.getSteamPlayer(caller.CSteamID);

                Vector3 d1 = otherPlayer.Player.transform.position;
                Vector3 vector31 = otherPlayer.Player.transform.rotation.eulerAngles;
                myPlayer.Player.sendTeleport(d1, MeasurementTool.angleToByte(vector31.y));
                ChatManager.say(caller.CSteamID, "Teleported to " + otherPlayer.SteamPlayerID.CharacterName);
            }
            else
            {
                ChatManager.say(caller.CSteamID, "Failed to find player");
            }
        }
    }
}
