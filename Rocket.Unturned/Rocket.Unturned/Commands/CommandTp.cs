using SDG;
using System;
using UnityEngine;
using System.Linq;
using Rocket.Unturned.Logging;
using Rocket.Core;
using Rocket.API;
using Rocket.Core.Translations;
using Rocket.Unturned.Player;

namespace Rocket.Unturned.Commands
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
            get { return "Teleports you to another player or location";}
        }

        public void Execute(RocketPlayer caller, string[] command)
        {
            if (command.Length != 1)
            {
                RocketChat.Say(caller, RocketTranslationManager.Translate("command_generic_invalid_parameter"));
                return;
            }

            if (caller.Stance == EPlayerStance.DRIVING || caller.Stance == EPlayerStance.SITTING)
            {
                RocketChat.Say(caller, RocketTranslationManager.Translate("command_generic_teleport_while_driving_error"));
                return;
            }


            RocketPlayer otherPlayer = RocketPlayer.FromName(command[0]);
            if (otherPlayer!=null && otherPlayer != caller)
            {
                Vector3 d1 = otherPlayer.Player.transform.position;
                Vector3 vector31 = otherPlayer.Player.transform.rotation.eulerAngles;
                caller.Teleport(otherPlayer);
                Logger.Log(RocketTranslationManager.Translate("command_tp_teleport_console", caller.CharacterName, otherPlayer.CharacterName));
                RocketChat.Say(caller, RocketTranslationManager.Translate("command_tp_teleport_private", otherPlayer.CharacterName));
            }
            else {
                Node item = LevelNodes.Nodes.Where(n => n.NodeType == ENodeType.Location && ((NodeLocation)n).Name.ToLower().Contains(command[0].ToLower())).FirstOrDefault();
                if (item != null)
                {
                    Vector3 c = item.Position + new Vector3(0f, 0.5f, 0f);
                    caller.Teleport(c, MeasurementTool.angleToByte(caller.Rotation));
                    Logger.Log(RocketTranslationManager.Translate("command_tp_teleport_console", caller.CharacterName, ((NodeLocation)item).Name));
                    RocketChat.Say(caller, RocketTranslationManager.Translate("command_tp_teleport_private", ((NodeLocation)item).Name));
                }
                else
                {
                    RocketChat.Say(caller, RocketTranslationManager.Translate("command_tp_failed_find_destination"));
                }
            }
        }
    }
}