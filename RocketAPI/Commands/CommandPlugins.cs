using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG;
using Rocket.RocketAPI.Interfaces;

namespace Rocket.RocketAPI.Commands
{
    class CommandCommands : RocketCommand
    {
        void RocketCommand.Execute(SteamPlayerID m, string s)
        {
            string message = "Plugins: " + string.Join(", ", RocketAPI.Plugins.plugins.Select(x => x.GetType().Assembly.GetName().Name).ToArray());
            ChatManager.say(m.CSteamId, message);
        }

        string RocketCommand.Name
        {
            get { return "plugins"; }
        }

        string RocketCommand.Help
        {
            get { return "Shows all plugins"; }
        }

    }
}
