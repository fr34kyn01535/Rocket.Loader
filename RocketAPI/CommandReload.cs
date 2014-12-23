using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG;

namespace Rocket.RocketAPI
{
    class CommandReload : Command
    {

        public CommandReload()
        {
            base.commandName = "reload";
            base.commandInfo = "Reload - Re-initializes all plugins";
            base.commandHelp = "Re-initializes all plugins";
        }

        protected override void execute(SteamPlayerID m, string s)
        {
            Bootstrap.InitializeBootstrap();
            Logger.Log("Reloaded plugins");
            ChatManager.say("Reloaded plugins");
        }

    }
}
