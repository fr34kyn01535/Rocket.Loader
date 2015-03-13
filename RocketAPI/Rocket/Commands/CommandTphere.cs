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
            if (!RocketCommand.IsPlayer(caller)) return;

            SteamPlayer otherPlayer = PlayerTool.getSteamPlayer(command);
            if (otherPlayer!=null && otherPlayer.SteamPlayerID.CSteamID.ToString() != caller.CSteamID.ToString())
            {
                SteamPlayer myPlayer = PlayerTool.getSteamPlayer(caller.CSteamID);

                Vector3 d1 = myPlayer.Player.transform.position;
                Vector3 vector31 = myPlayer.Player.transform.rotation.eulerAngles;
                otherPlayer.Player.sendTeleport(d1, MeasurementTool.angleToByte(vector31.y));
                Logger.Log(RocketTranslation.Translate("command_tphere_teleport_console", otherPlayer.SteamPlayerID.CharacterName, myPlayer.SteamPlayerID.CharacterName));
                RocketChatManager.Say(caller.CSteamID,RocketTranslation.Translate("command_tphere_teleport_from_private",otherPlayer.SteamPlayerID.CharacterName));
                RocketChatManager.Say(otherPlayer.SteamPlayerID.CSteamID, RocketTranslation.Translate("command_tphere_teleport_to_private",myPlayer.SteamPlayerID.CharacterName));
            }
            else
            {
                RocketChatManager.Say(caller.CSteamID, RocketTranslation.Translate("command_tphere_failed_find_player"));
            }
        }
    }
}