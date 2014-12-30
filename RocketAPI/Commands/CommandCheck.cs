using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG;

namespace Rocket.RocketAPI.Commands
{
    class CommandCheck : RocketCommand
    {
        public void Execute(SteamPlayerID caller, string command)
        {
            try
            {
                SteamPlayerID steamPlayerID = null;
                SteamPlayerlist.tryGetPlayer(command, out steamPlayerID);
                if (steamPlayerID == null)
                {
                    return;
                }
                SteamPlayer steamPlayer = PlayerTool.getSteamPlayer(steamPlayerID.SteamId);

                steamPlayer.Player.playerLife.tellWater(steamPlayerID.SteamId, (byte)66);
                steamPlayer.Player.playerLife.tellHealth(steamPlayerID.SteamId, (byte)66);
                System.Threading.Thread.Sleep(1000);
                if (
                    steamPlayer.Player.playerLife.a == false &&
                    steamPlayer.Player.playerLife.f == (byte)100 &&
                    steamPlayer.Player.playerLife.g == (byte)100 &&
                    steamPlayer.Player.playerLife.i == false &&
                    steamPlayer.Player.playerLife.N == (byte)0 &&
                    steamPlayer.Player.playerLife.o == (byte)100 &&
                    steamPlayer.Player.playerLife.q == (byte)100 &&
                    steamPlayer.Player.playerLife.V == (byte)100 &&
                    steamPlayer.Player.playerLife.y == (byte)100
                    )
                {
                    string msg = steamPlayer.Player.name + " is probably hacking...";
                    Logger.Log(msg);
                    ChatManager.say(caller.SteamId, msg);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
            }
        }


        public string Name
        {
            get { return "Check"; }
        }

        public string Help
        {
            get { return "Checks if the player is a hacker"; }
        }
    }
}
