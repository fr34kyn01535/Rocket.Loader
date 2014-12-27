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
            base.commandName = "Plugins";
            base.commandInfo = "Plugins - shows all plugins";
            base.commandHelp = "Shows all plugins";
        }

        protected override void execute(SteamPlayerID m, string s)
        {
            string message = "Plugins: " + string.Join(",", Core.Plugins.Select(x => x.GetType().Assembly.GetName().Name).ToArray());
            ChatManager.say(m.SteamId, message);
        }

    }
}
