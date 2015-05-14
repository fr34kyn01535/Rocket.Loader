using SDG;
using System;
using Rocket.API;

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
                RocketChatManager.Say(caller, RocketTranslation.Translate("command_clear_private"));
            }
            else
            {
                if (caller != null && !caller.HasPermission("ci.others")) return;
                RocketPlayer player = RocketPlayer.FromName(command[0]);
                if (player == null)
                {
                    RocketChatManager.Say(caller, RocketTranslation.Translate("command_generic_failed_find_player"));
                    return;
                }
                if (!player.Inventory.Clear())
                {
                    RocketChatManager.Say(caller, RocketTranslation.Translate("command_clear_error", player.CharacterName + "'s"));
                    return;
                }
                RocketChatManager.Say(caller, RocketTranslation.Translate("command_clear_other_success", player.CharacterName + "'s"));
                RocketChatManager.Say(player, RocketTranslation.Translate("command_clear_other", caller.CharacterName));
            }
        }
    }
}