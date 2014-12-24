using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG;

namespace Rocket.RocketAPI
{
    class CommandPlugins : Command
    {

        public CommandPlugins()
        {
            base.commandName = "plugins";
            base.commandInfo = "plugins - shows all plugins";
            base.commandHelp = "Shows all plugins";
        }

        protected override void execute(SteamPlayerID m, string s)
        {
            string message = "The following plugins are currently loaded: " + string.Join(",", Core.Plugins.Select(x => x.Name).ToArray());
            Logger.Log(message);
            ChatManager.say(m.G, message);
        }

    }
}
