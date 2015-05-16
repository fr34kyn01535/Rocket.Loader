using SDG;
using System;
using Rocket.API;
using Rocket.Unturned.Logging;
using Rocket.Core;
using Rocket.Core.Translations;
using Rocket.Unturned.Player;

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

        public void Execute(RocketPlayer caller, string[] command)
        {
            if (command.Length == 0)
            {
                if (!caller.Inventory.Clear())
                {
                    Logger.Log("Something went wrong removing " + caller.CharacterName + "'s clothing!");
                }
                RocketChat.Say(caller, RocketTranslationManager.Translate("command_clear_private"));
            }
            else
            {
                if (caller != null && !caller.HasPermission("ci.others")) return;
                RocketPlayer player = RocketPlayer.FromName(command[0]);
                if (player == null)
                {
                    RocketChat.Say(caller, RocketTranslationManager.Translate("command_generic_failed_find_player"));
                    return;
                }
                if (!player.Inventory.Clear())
                {
                    RocketChat.Say(caller, RocketTranslationManager.Translate("command_clear_error", player.CharacterName + "'s"));
                    return;
                }
                RocketChat.Say(caller, RocketTranslationManager.Translate("command_clear_other_success", player.CharacterName + "'s"));
                RocketChat.Say(player, RocketTranslationManager.Translate("command_clear_other", caller.CharacterName));
            }
        }
    }
}