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



            SteamPlayer fromPlayer = PlayerTool.getSteamPlayer(caller.CSteamId);


            SteamPlayerID steamPlayerID = null;
            SteamPlayerlist.tryGetPlayer(command, out steamPlayerID);
            if (steamPlayerID == null)
            {
                return;
            }

            Logger.Log("ok");
            SteamPlayer toPlayer = PlayerTool.getSteamPlayer(steamPlayerID.CSteamId);

                Logger.Log("ok2" + toPlayer.Player.name);
                Vector3 d1 = toPlayer.Player.transform.position;
                Logger.Log("ok1");
                Vector3 vector31 = toPlayer.Player.transform.rotation.eulerAngles;
                fromPlayer.Player.sendTeleport(d1, MeasurementTool.angleToByte(vector31.y));
            
            Logger.Log("xx:");
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
