using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG;

namespace Rocket.RocketAPI.Commands
{
    /// <summary>
    /// test
    /// </summary>
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
            get { return "Commands"; }
        }

        public string Help
        {
            get { return "Shows all custom commands"; }
        }
    }
}
