using Rocket.Core.Logging;
using Rocket.Unturned.Player;
using System.Collections.Generic;

namespace Rocket.Unturned.Commands
{
    public class CommandCi : IRocketCommand
    {
        public bool RunFromConsole
        {
            get { return false; }
        }

        public string Name
        {
            get { return "ci"; }
        }

        public string Help
        {
            get { return "Clears your inventory"; }
        }

        public string Syntax
        {
            get { return "[player]"; }
        }

        public List<string> Aliases
        {
            get { return new List<string>() { "clearinventory" }; }
        }

        public void Execute(UnturnedPlayer caller, string[] command)
        {
            if (command.Length == 0)
            {
                if (!caller.Inventory.Clear())
                {
                    Logger.Log("Something went wrong removing " + caller.CharacterName + "'s clothing!");
                }
                RocketChat.Say(caller, U.Translate("command_clear_private"));
            }
            else
            {
                if (caller != null && !caller.HasPermission("ci.others")) return;
                UnturnedPlayer player = UnturnedPlayer.FromName(command[0]);
                if (player == null)
                {
                    RocketChat.Say(caller, U.Translate("command_generic_failed_find_player"));
                    return;
                }
                if (!player.Inventory.Clear())
                {
                    RocketChat.Say(caller, U.Translate("command_clear_error", player.CharacterName + "'s"));
                    return;
                }
                RocketChat.Say(caller, U.Translate("command_clear_other_success", player.CharacterName + "'s"));
                RocketChat.Say(player, U.Translate("command_clear_other", caller.CharacterName));
            }
        }
    }
}