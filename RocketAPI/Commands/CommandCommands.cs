using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG;
using Rocket.RocketAPI.Interfaces;

namespace Rocket.RocketAPI.Commands
{
    class CommandPlugins : RocketCommand
    {
        public void Execute(SteamPlayerID m, string s)
        {
            ChatManager.say(m.CSteamId, "Commands: ");
            foreach (RocketCommand cmd in RocketAPI.Commands.commands)
            {
                ChatManager.say(m.CSteamId, cmd.Name+": " +cmd.Help);
            }
        }

        public string Name
        {
            get { return "commands"; }
        }

        public string Help
        {
            get { return "Shows all custom commands"; }
        }
    }
}
