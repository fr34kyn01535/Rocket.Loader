using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG;

namespace Rocket.RocketAPI.Commands
{
    class CommandPlugins : RocketCommand
    {
        public void Execute(SteamPlayerID m, string s)
        {
            ChatManager.say(m.SteamId, "Commands: ");
            foreach (RocketCommand cmd in RocketAPI.Commands.commands)
            {
                ChatManager.say(m.SteamId, cmd.Name+": " +cmd.Help);
            }
        }

        public string Name
        {
            get { return "Commands"; }
        }

        public string Help
        {
            get { return "Shows all custom commands"; }
        }
    }
}
