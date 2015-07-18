using Rocket.Core.Logging;
using Rocket.Unturned.Player;
using System.Collections.Generic;

namespace Rocket.Unturned.Commands
{
    public class CommandVanish : IRocketCommand
    {
        public bool RunFromConsole
        {
            get { return false; }
        }

        public string Name
        {
            get { return "vanish"; }
        }

        public string Help
        {
            get { return "Are we rushing in or are we goin' sneaky beaky like?";}
        }

        public string Syntax
        {
            get { return ""; }
        }

        public List<string> Aliases
        {
            get { return new List<string>(); }
        }

        public void Execute(UnturnedPlayer caller, string[] command)
        {
            if (caller.Features.VanishMode)
            {
                Logger.Log(U.Translate("command_vanish_disable_console", caller.CharacterName));
                RocketChat.Say(caller, U.Translate("command_vanish_disable_private"));
                caller.Features.VanishMode = false;
            }
            else
            {
                Logger.Log(U.Translate("command_vanish_enable_console", caller.CharacterName));
                RocketChat.Say(caller, U.Translate("command_vanish_enable_private"));
                caller.Features.VanishMode = true;
            }
        }
    }
}