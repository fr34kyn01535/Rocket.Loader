using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG;
using UnityEngine;
using Rocket.RocketAPI.Interfaces;

namespace Rocket.RocketAPI.Commands
{
    class CommandReload : RocketCommand
    {
        void RocketCommand.Execute(SteamPlayerID caller, string command)
        {
            Bootstrap.RocketAPI.Reload();
            Logger.Log("Reloaded Rocket");
            ChatManager.say("Reloaded Rocket");
        }

        string RocketCommand.Name
        {
            get { return "Reload"; }
        }

        string RocketCommand.Help
        {
            get { return "Re-initializes all plugins"; }
        }
    }
}
