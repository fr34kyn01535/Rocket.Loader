using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG;
using UnityEngine;
using Rocket.RocketAPI.Interfaces;

namespace Rocket.RocketAPI.Commands
{
    class CommandTeleport : RocketCommand
    {
        public void Execute(SteamPlayerID caller, string command)
        {



            SteamPlayer steamPlayer;
            if (SteamPlayerlist.tryGetSteamPlayer(command, out steamPlayer))
            {
                ChatManager.say(caller.CSteamId, "test");
            }
            else
            {
                ChatManager.say(caller.CSteamId, "Failed to find player");
            }








            string[] commandArray = command.Split(' ');

            if (commandArray.Length < 2)
            {
                ChatManager.say(caller.CSteamId, "Missing arguments");
                return;
            }

            Logger.Log("ok");
            SteamPlayer fromPlayer = PlayerTool.getSteamPlayer(caller.CSteamId);

            Logger.Log("fromthere" + command);

            SteamPlayerID toPlayerID;
            if (SteamPlayerlist.tryGetPlayer(command.ToLower().Replace(Name.ToLower() + " ", ""), out toPlayerID))
            {
                SteamPlayer toPlayer;
                if (SteamPlayerlist.tryGetSteamPlayer(toPlayerID.CSteamId.ToString(), out toPlayer))
                {
                    Vector3 d1 = toPlayer.Player.transform.position;
                    Logger.Log("ok1");
                    Vector3 vector31 = toPlayer.Player.transform.rotation.eulerAngles;
                    fromPlayer.Player.sendTeleport(d1, MeasurementTool.angleToByte(vector31.y));

                    Logger.Log("xx:");
                }
            }
            else
            {
                ChatManager.say(caller.CSteamId, "Failed to find player");
            }


        }


        public string Name
        {
            get { return "tp"; }
        }

        public string Help
        {
            get { return "Teleports you to another player"; }
        }
    }
}
