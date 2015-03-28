using Rocket.Logging;
using Rocket.RocketAPI;
using SDG;
using System;
using UnityEngine;
using System.Linq;

namespace Rocket.Commands
{
    public class CommandTp : IRocketCommand
    {
        public bool RunFromConsole
        {
            get { return false; }
        }

        public string Name
        {
            get { return "tp"; }
        }

        public string Help
        {
            get { return "Teleports you to another player";}
        }

        public void Execute(Steamworks.CSteamID caller, string command)
        {
            SteamPlayer myPlayer = PlayerTool.getSteamPlayer(caller);

            if(String.IsNullOrEmpty(command)){
                RocketChatManager.Say(caller, RocketTranslation.Translate("command_generic_invalid_parameter"));
                return;
            }

            if (myPlayer.Player.Stance.Stance == EPlayerStance.DRIVING || myPlayer.Player.Stance.Stance == EPlayerStance.SITTING)
            {
                RocketChatManager.Say(caller, RocketTranslation.Translate("command_generic_teleport_while_driving_error"));
                return;
            }


            SteamPlayer otherPlayer = PlayerTool.getSteamPlayer(command);
            if (otherPlayer!=null && otherPlayer.SteamPlayerID.CSteamID.ToString() != caller.ToString())
            {
                Vector3 d1 = otherPlayer.Player.transform.position;
                Vector3 vector31 = otherPlayer.Player.transform.rotation.eulerAngles;
                myPlayer.Player.sendTeleport(d1, MeasurementTool.angleToByte(vector31.y));
                Logger.Log(RocketTranslation.Translate("command_tp_teleport_console", myPlayer.SteamPlayerID.CharacterName, otherPlayer.SteamPlayerID.CharacterName));
                RocketChatManager.Say(caller, RocketTranslation.Translate("command_tp_teleport_private", otherPlayer.SteamPlayerID.CharacterName));
            }
            else {
                Node item = LevelNodes.Nodes.Where(n => n.NodeType == ENodeType.Location && ((NodeLocation)n).Name.ToLower().Contains(command.ToLower())).FirstOrDefault();
                if (item != null)
                {
                    Vector3 c = item.Position + new Vector3(0f, 0.5f, 0f);
                    Vector3 vector31 = myPlayer.Player.transform.rotation.eulerAngles;
                    myPlayer.Player.sendTeleport(c, MeasurementTool.angleToByte(vector31.y));
                    Logger.Log(RocketTranslation.Translate("command_tp_teleport_console", myPlayer.SteamPlayerID.CharacterName, ((NodeLocation)item).Name));
                    RocketChatManager.Say(caller, RocketTranslation.Translate("command_tp_teleport_private", ((NodeLocation)item).Name));
                }
                else
                {
                    RocketChatManager.Say(caller, RocketTranslation.Translate("command_tp_failed_find_destination"));
                }
            }
        }
    }
}