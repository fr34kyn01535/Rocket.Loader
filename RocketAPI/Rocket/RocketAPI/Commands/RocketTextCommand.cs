using Rocket.Logging;
using Rocket.RocketAPI;
using SDG;
using System;
using System.Collections.Generic;

namespace Rocket.RocketAPI
{
    public class RocketTextCommand : Command
    {
        private List<string> text;

        public RocketTextCommand(string commandName,string commandHelp,List<string> text)
        {
            base.commandName = commandName;
            base.commandHelp = commandHelp;
            base.commandInfo = base.commandName + " - " + base.commandHelp;
            this.text = text;
        }

        protected override void execute(Steamworks.CSteamID caller, string command)
        {
            foreach (string l in text) {
                RocketChatManager.Say(caller, l);
            }
        }
    }
}