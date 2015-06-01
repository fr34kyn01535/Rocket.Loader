using SDG;
using System;
using UnityEngine;
using System.Linq;
using Rocket.Unturned.Logging;
using Rocket.Core;
using Rocket.API;
using Rocket.Core.Translations;
using Rocket.Unturned.Player;
using System.Collections.Generic;

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

        public string Syntax
        {
            get { return "<player | place | x y z>"; }
        }

        public List<string> Aliases
        {
            get { return new List<string>(); }
        }

        public void Execute(RocketPlayer caller, string[] command)
        {
            if (command.Length != 1 && command.Length != 3)
            {
                RocketChat.Say(caller, RocketTranslationManager.Translate("command_generic_invalid_parameter"));
                return;
            }

            if (caller.Stance == EPlayerStance.DRIVING || caller.Stance == EPlayerStance.SITTING)
            {
                RocketChat.Say(caller, RocketTranslationManager.Translate("command_generic_teleport_while_driving_error"));
                return;
            }

            float? x = null;
            float? y = null;
            float? z = null;

            if (command.Length == 3)
            {
                x = command.GetFloatParameter(0);
                y = command.GetFloatParameter(1);
                z = command.GetFloatParameter(2);
            }
            if (x != null && y != null && z != null)
            {
                caller.Teleport(new Vector3((float)x, (float)y, (float)z), MeasurementTool.angleToByte(caller.Rotation));
                Logger.Log(RocketTranslationManager.Translate("command_tp_teleport_console", caller.CharacterName, (float)x + "," + (float)y + "," + (float)z));
                RocketChat.Say(caller, RocketTranslationManager.Translate("command_tp_teleport_private", (float)x + "," + (float)y + "," + (float)z));
            }
            else
            {

                RocketPlayer othercaller = RocketPlayer.FromName(command[0]);
                if (othercaller != null && othercaller != caller)
                {
                    Vector3 d1 = othercaller.Player.transform.position;
                    Vector3 vector31 = othercaller.Player.transform.rotation.eulerAngles;
                    caller.Teleport(othercaller);
                    Logger.Log(RocketTranslationManager.Translate("command_tp_teleport_console", caller.CharacterName, othercaller.CharacterName));
                    RocketChat.Say(caller, RocketTranslationManager.Translate("command_tp_teleport_private", othercaller.CharacterName));
                }
                else
                {
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
}