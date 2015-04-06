using Rocket.Logging;
using Rocket.RocketAPI;
using SDG;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace Rocket.Commands
{
    public class CommandRocket : IRocketCommand
    {
        public bool RunFromConsole
        {
            get { return true; }
        }

        public string Name
        {
            get { return "rocket"; }
        }

        public string Help
        {
            get { return "About us :)";}
        }

        public void Execute(RocketPlayer caller, string command)
        {
            RocketChatManager.Say(caller, "Rocket v" + Assembly.GetExecutingAssembly().GetName().Version + " for Unturned v" + Steam.Version);
            RocketChatManager.Say(caller, "https://rocket.foundation © 2015");
        }
    }
}