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

        public void Execute(RocketPlayer caller, string command)
        {
            if(String.IsNullOrEmpty(command)){
                RocketChatManager.Say(caller, RocketTranslation.Translate("command_generic_invalid_parameter"));
                return;
            }

            if (caller.Stance == EPlayerStance.DRIVING || caller.Stance == EPlayerStance.SITTING)
            {
                RocketChatManager.Say(caller, RocketTranslation.Translate("command_generic_teleport_while_driving_error"));
                return;
            }


            RocketPlayer otherPlayer = RocketPlayer.FromName(command);
            if (otherPlayer!=null && otherPlayer != caller)
            {
                Vector3 d1 = otherPlayer.Player.transform.position;
                Vector3 vector31 = otherPlayer.Player.transform.rotation.eulerAngles;
                caller.Teleport(otherPlayer);
                Logger.Log(RocketTranslation.Translate("command_tp_teleport_console", caller.CharacterName, otherPlayer.CharacterName));
                RocketChatManager.Say(caller, RocketTranslation.Translate("command_tp_teleport_private", otherPlayer.CharacterName));
            }
            else {
                Node item = LevelNodes.Nodes.Where(n => n.NodeType == ENodeType.Location && ((NodeLocation)n).Name.ToLower().Contains(command.ToLower())).FirstOrDefault();
                if (item != null)
                {
                    Vector3 c = item.Position + new Vector3(0f, 0.5f, 0f);
                    caller.Teleport(c, MeasurementTool.angleToByte(caller.Rotation));
                    Logger.Log(RocketTranslation.Translate("command_tp_teleport_console", caller.CharacterName, ((NodeLocation)item).Name));
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