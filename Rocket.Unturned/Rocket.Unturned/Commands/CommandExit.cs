using Rocket.API;
using Rocket.Core;
using Rocket.Core.Translations;
using Rocket.Unturned.Logging;
using Rocket.Unturned.Player;
using SDG;
using System;

namespace Rocket.Unturned.Commands
{
    public class CommandExit : IRocketCommand
    {
        public bool RunFromConsole
        {
            get { return false; }
        }

        public string Name
        {
            get { return "exit"; }
        }

        public string Help
        {
            get { return "Exit the game without cooldown";}
        }

        public string Syntax
        {
            get { return ""; }
        }

        public void Execute(RocketPlayer caller, string[] command)
        {
            Steam.kick(caller.CSteamID, "");
        }
    }
}