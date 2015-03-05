using Rocket.Logging;
using Rocket.RocketAPI;
using SDG;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace Rocket.Commands
{
    public class CommandRocket : Command
    {
        public CommandRocket()
        {
            base.commandName = "rocket";
            base.commandHelp = "About us :)";
            base.commandInfo = base.commandName + " - " + base.commandHelp;
        }

        protected override void execute(SteamPlayerID caller, string command)
        {
            RocketChatManager.Say(caller.CSteamID, "Rocket v" + Assembly.GetExecutingAssembly().GetName().Version + " for Unturned v" + Steam.Version);
            RocketChatManager.Say(caller.CSteamID, "https://github.com/RocketFoundation/Rocket/wiki/Contributors");
            RocketChatManager.Say(caller.CSteamID, "https://rocket.foundation © 2015");
        }
    }
}