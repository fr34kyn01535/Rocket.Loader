using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG;

namespace Rocket.RocketAPI.Commands
{
    class CommandCommands : RocketCommand
    {
        public void Execute(SteamPlayerID m, string s)
        {
            string message = "Plugins: " + string.Join(", ", RocketAPI.Plugins.plugins.Select(x => x.GetType().Assembly.GetName().Name).ToArray());
            ChatManager.say(m.SteamId, message);
        }

        public string Name
        {
            get { return "Plugins"; }
        }

        public string Help
        {
            get { return "Shows all plugins"; }
        }
    }
}
