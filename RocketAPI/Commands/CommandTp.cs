using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG;
using UnityEngine;
using Rocket.RocketAPI.Interfaces;

namespace Rocket.RocketAPI.Commands
{
    class CommandTp : RocketCommand
    {
        public void Execute(SteamPlayerID caller, string command)
        {
            string[] commandArray = command.Split(' ');

            if (commandArray.Length < 2)
            {
                ChatManager.say(caller.CSteamId, "Missing arguments");
                return;
            }

            string message = "";
            if (commandArray.Length > 2)
            {
                for (int i = 2; i < commandArray.Length; i++)
                {
                    if (i != 2) message += " ";
                    message += commandArray[i];
                }
            }

            SteamPlayer otherPlayer;
            if (SteamPlayerlist.tryGetSteamPlayer(command.Replace("/" + Name + " ", ""), out otherPlayer))
            {
                SteamPlayer myPlayer = PlayerTool.getSteamPlayer(caller.CSteamId);

                Vector3 d1 = otherPlayer.Player.transform.position;
                Vector3 vector31 = otherPlayer.Player.transform.rotation.eulerAngles;
                myPlayer.Player.sendTeleport(d1, MeasurementTool.angleToByte(vector31.y));
                ChatManager.say(caller.CSteamId, "Teleported to " + otherPlayer.SteamPlayerId.IngameName);

            }
            else
            {
                ChatManager.say(caller.CSteamId, "Failed to find player");
            }

        }


        public string Name
        {
            get { return "Tp"; }
        }

        public string Help
        {
            get { return "Teleports you to another player"; }
        }
    }
}
