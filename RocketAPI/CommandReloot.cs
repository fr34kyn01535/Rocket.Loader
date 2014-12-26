using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG;

namespace Rocket.RocketAPI
{
    class CommandReloot : Command
    {

        public CommandReloot()
        {
            base.commandName = "Reloot";
            base.commandInfo = "Reloot - Resets all lootspawns";
            base.commandHelp = "Resets all lootspawns";
        }

        protected override void execute(SteamPlayerID m, string s)
        {
            ItemManager t = new ItemManager();
            //t.(1);

            //ChatManager.say(m.B, "Successfully relooted!");
        }

    }
}
