using Rocket.API;
using Rocket.Core;
using Rocket.Core.Permissions;
using Rocket.Core.Translations;
using Rocket.Unturned.Logging;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;

namespace Rocket.Unturned.Commands
{
    public class CommandP : IRocketCommand
    {
        public bool RunFromConsole
        {
            get { return false; }
        }

        public string Name
        {
            get { return "p"; }
        }

        public string Help
        {
            get { return "Lists permissions";}
        }

        public string Syntax
        {
            get { return "<player> [group]"; }
        }

        public List<string> Aliases
        {
            get { return new List<string>() { "permissions" }; }
        }

        public void Execute(RocketPlayer caller, string[] command)
        {
            RocketPlayer player = command.GetRocketPlayerParameter(0);
            string groupName = command.GetStringParameter(1);

            if (command.Length == 0)
            {
                RocketChat.Say(caller, RocketTranslationManager.Translate("command_p_groups_private", "Your", String.Join(", ", RocketPermissionsManager.GetDisplayGroups(caller.CSteamID.ToString()))));
                RocketChat.Say(caller, RocketTranslationManager.Translate("command_p_permissions_private", "Your", String.Join(", ", RocketPermissionsManager.GetPermissions(caller.CSteamID.ToString()).ToArray())));
            }
            else if(command.Length == 1 && player != null) {
                RocketChat.Say(caller, RocketTranslationManager.Translate("command_p_groups_private", player.CharacterName+"s", String.Join(", ", RocketPermissionsManager.GetDisplayGroups(player.CSteamID.ToString()))));
                RocketChat.Say(caller, RocketTranslationManager.Translate("command_p_permissions_private", player.CharacterName+"s", String.Join(", ", RocketPermissionsManager.GetPermissions(player.CSteamID.ToString()).ToArray())));
            }
            else if (command.Length == 2 && player != null && !String.IsNullOrEmpty(groupName))
            {
                if (RocketPermissionsManager.SetGroup(player.CSteamID.ToString(), groupName))
                {
                    RocketChat.Say(caller, RocketTranslationManager.Translate("command_p_group_assigned", player.CharacterName, groupName));
                }
                else {
                    RocketChat.Say(caller, RocketTranslationManager.Translate("command_p_group_not_found"));
                }
            }
            else
            {
                RocketChat.Say(caller, RocketTranslationManager.Translate("command_generic_invalid_parameter"));
                return;
            }

            
         }
    }
}