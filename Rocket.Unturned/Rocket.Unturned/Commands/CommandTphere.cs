using Rocket.Core.Logging;
using Rocket.Unturned.Player;
using System.Collections.Generic;

namespace Rocket.Unturned.Commands
{
    internal class CommandTphere : IRocketCommand
    {
        public bool RunFromConsole
        {
            get { return false; }
        }

        public string Name
        {
            get { return "tphere"; }
        }

        public string Help
        {
            get { return "Teleports another player to you";}
        }

        public string Syntax
        {
            get { return "<player>"; }
        }

        public List<string> Aliases
        {
            get { return new List<string>(); }
        }

        public void Execute(UnturnedPlayer caller, string[] command)
        {
            if (command.Length != 1)
            {
                RocketChat.Say(caller, U.Translate("command_generic_invalid_parameter"));
                return;
            }
            UnturnedPlayer otherPlayer = UnturnedPlayer.FromName(command[0]);
            if (otherPlayer!=null && otherPlayer != caller)
            {
                otherPlayer.Teleport(caller);
                Logger.Log(U.Translate("command_tphere_teleport_console", otherPlayer.CharacterName, caller.CharacterName));
                RocketChat.Say(caller, U.Translate("command_tphere_teleport_from_private", otherPlayer.CharacterName));
                RocketChat.Say(otherPlayer, U.Translate("command_tphere_teleport_to_private", caller.CharacterName));
            }
            else
            {
                RocketChat.Say(caller, U.Translate("command_generic_failed_find_player"));
            }
        }
    }
}