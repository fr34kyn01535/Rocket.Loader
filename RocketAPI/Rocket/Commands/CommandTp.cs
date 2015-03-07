using Rocket.Logging;
using Rocket.RocketAPI;
using SDG;
using System;
using UnityEngine;
using System.Linq;

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
            SteamPlayer myPlayer = PlayerTool.getSteamPlayer(caller.CSteamID);

            
            if(String.IsNullOrEmpty(command)){
                RocketChatManager.Say(caller.CSteamID, "Invalid Parameter");
                return;
            }

            if (myPlayer.Player.Stance.Stance == EPlayerStance.DRIVING || myPlayer.Player.Stance.Stance == EPlayerStance.SITTING)
            {
                RocketChatManager.Say(caller.CSteamID, "You can't teleport while you are driving");
                return;
            } 


            if (SteamPlayerlist.tryGetSteamPlayer(command, out otherPlayer) && otherPlayer.SteamPlayerID.CSteamID.ToString() != caller.CSteamID.ToString())
            {

                
                Vector3 d1 = otherPlayer.Player.transform.position;
                Vector3 vector31 = otherPlayer.Player.transform.rotation.eulerAngles;
                myPlayer.Player.sendTeleport(d1, MeasurementTool.angleToByte(vector31.y));
                Logger.Log(caller.CharacterName + " teleported to " + otherPlayer.SteamPlayerID.CharacterName);
                RocketChatManager.Say(caller.CSteamID, "Teleported to " + otherPlayer.SteamPlayerID.CharacterName);
            }
            else {
                Node item = LevelNodes.Nodes.Where(n => n.NodeType == ENodeType.Location && ((NodeLocation)n).Name.ToLower().Contains(command.ToLower())).FirstOrDefault();
                if (item != null)
                {
                    Vector3 c = item.Position + new Vector3(0f, 0.5f, 0f);
                    Vector3 vector31 = myPlayer.Player.transform.rotation.eulerAngles;
                    myPlayer.Player.sendTeleport(c, MeasurementTool.angleToByte(vector31.y));
                    Logger.Log(caller.CharacterName + " teleported to " + ((NodeLocation)item).Name);
                    RocketChatManager.Say(caller.CSteamID, "Teleported to " + ((NodeLocation)item).Name);
                }
                else
                {
                    RocketChatManager.Say(caller.CSteamID, "Failed to find destination");
                }
            }
        }
    }
}