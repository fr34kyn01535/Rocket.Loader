using SDG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rocket.RocketAPI
{
    public class RocketCommand
    {
        public static bool IsPlayer(SteamPlayerID caller){
            return (caller.CSteamID != null && !String.IsNullOrEmpty(caller.CSteamID.ToString()) && caller.CSteamID.ToString() != "0");
        }
    }
}
