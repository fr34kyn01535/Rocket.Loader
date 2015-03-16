using Rocket.Logging;
using Rocket.RocketAPI;
using SDG;
using System;
using System.Collections.Generic;

namespace Rocket.Commands
{
    public class RocketTextCommand : Command
    {
        private List<string> text;

        public RocketTextCommand(string commandName,string commandHelp,string commandInfo,List<string> text)
        {
            base.commandName = commandName;
            base.commandHelp = commandHelp;
            base.commandInfo = commandInfo;
            this.text = text;
        }

        protected override void execute(SteamPlayerID caller, string command)
        {
            foreach (string l in text) {
                RocketChatManager.Say(caller.CSteamID, l);
            }
        }
    }
}