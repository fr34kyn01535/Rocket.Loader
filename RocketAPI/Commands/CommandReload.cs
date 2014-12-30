using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG;
using UnityEngine;

namespace Rocket.RocketAPI.Commands
{
    public class CommandReload : RocketCommand
    {
        public void Execute(SteamPlayerID caller, string command)
        {
            Bootstrap.RocketAPI.Reload();
            Logger.Log("Reloaded Rocket");
            ChatManager.say("Reloaded Rocket");
        }

        public string Name
        {
            get { return "Reload"; }
        }

        public string Help
        {
            get { return "Re-initializes all plugins"; }
        }
    }
}
