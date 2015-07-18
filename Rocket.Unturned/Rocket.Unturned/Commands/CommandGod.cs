using Rocket.Core.Logging;
using Rocket.Unturned.Player;
using System.Collections.Generic;

namespace Rocket.Unturned.Commands
{
    public class CommandGod : IRocketCommand
    {
        public bool RunFromConsole
        {
            get { return false; }
        }

        public string Name
        {
            get { return "god"; }
        }

        public string Help
        {
            get { return "Cause you ain't givin a shit";}
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
            if (caller.Features.GodMode)
            {
                Logger.Log(U.Translate("command_god_disable_console", caller.CharacterName));
                RocketChat.Say(caller, U.Translate("command_god_disable_private"));
                caller.Features.GodMode = false;
            }
            else
            {
                Logger.Log(U.Translate("command_god_enable_console", caller.CharacterName));
                RocketChat.Say(caller, U.Translate("command_god_enable_private"));
                caller.Features.GodMode = true;
            }
        }
    }
}