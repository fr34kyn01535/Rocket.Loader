using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG;
using UnityEngine;

namespace Rocket.RocketAPI.Commands
{
    class CommandTeleport : RocketCommand
    {
        public void Execute(SteamPlayerID caller, string command)
        {
            SteamPlayerID steamPlayerID = null;
            SteamPlayerlist.tryGetPlayer(command, out steamPlayerID);
            Logger.Log("y:"+steamPlayerID);
            if (steamPlayerID != null)
            {
                Logger.Log("x");
                SteamPlayer fromPlayer = PlayerTool.getSteamPlayer(caller.SteamId);
                SteamPlayer toPlayer = PlayerTool.getSteamPlayer(steamPlayerID.SteamId);
                Logger.Log("ok");
                Logger.Log("ok2" + toPlayer.Player.name);
                Vector3 d1 = toPlayer.Player.transform.position;
                Logger.Log("ok1");
                Vector3 vector31 = toPlayer.Player.transform.rotation.eulerAngles;
                fromPlayer.Player.sendTeleport(d1, MeasurementTool.angleToByte(vector31.y));
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
