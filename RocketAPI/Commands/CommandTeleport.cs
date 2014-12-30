using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG;
using UnityEngine;

namespace Rocket.RocketAPI.Commands
{
    class CommandCheck : RocketCommand
    {
        public void Execute(SteamPlayerID caller, string command)
        {
            SteamPlayerID steamPlayerID = null;
            SteamPlayerlist.tryGetPlayer(command, out steamPlayerID);
            if (steamPlayerID == null)
            {
                return;
            }

            SteamPlayer fromPlayer = PlayerTool.getSteamPlayer(caller.SteamId);
            SteamPlayer toPlayer = PlayerTool.getSteamPlayer(steamPlayerID.SteamId);

            Vector3 d1 = toPlayer.Player.transform.position;
            Vector3 vector31 = toPlayer.Player.transform.rotation.eulerAngles;
            fromPlayer.Player.sendTeleport(d1, MeasurementTool.angleToByte(vector31.y));
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
