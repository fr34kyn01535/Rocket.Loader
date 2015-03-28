using Rocket.Logging;
using Rocket.RocketAPI;
using SDG;
using System;
using UnityEngine;

namespace Rocket.Commands
{
    internal class CommandTphere : IRocketCommand
    {
        public bool RunFromConsole
        {
            get { return false; }
        }

        public string Name
        {
            get { return "tphere"; }
        }

        public string Help
        {
            get { return "Teleports another player to you";}
        }

        public void Execute(Steamworks.CSteamID caller, string command)
        {
            SteamPlayer otherPlayer = PlayerTool.getSteamPlayer(command);
            if (otherPlayer!=null && otherPlayer.SteamPlayerID.CSteamID.ToString() != caller.ToString())
            {
                SteamPlayer myPlayer = PlayerTool.getSteamPlayer(caller);

                Vector3 d1 = myPlayer.Player.transform.position;
                Vector3 vector31 = myPlayer.Player.transform.rotation.eulerAngles;
                otherPlayer.Player.sendTeleport(d1, MeasurementTool.angleToByte(vector31.y));
                Logger.Log(RocketTranslation.Translate("command_tphere_teleport_console", otherPlayer.SteamPlayerID.CharacterName, myPlayer.SteamPlayerID.CharacterName));
                RocketChatManager.Say(caller,RocketTranslation.Translate("command_tphere_teleport_from_private",otherPlayer.SteamPlayerID.CharacterName));
                RocketChatManager.Say(otherPlayer.SteamPlayerID.CSteamID, RocketTranslation.Translate("command_tphere_teleport_to_private",myPlayer.SteamPlayerID.CharacterName));
            }
            else
            {
                RocketChatManager.Say(caller, RocketTranslation.Translate("command_generic_failed_find_player"));
            }
        }
    }
}