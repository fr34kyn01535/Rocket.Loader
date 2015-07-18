using Rocket.Core;
using Rocket.Unturned.Player;
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

        public void Execute(UnturnedPlayer caller, string[] command)
        {
            UnturnedPlayer player = command.GetUnturnedPlayerParameter(0);
            string groupName = command.GetStringParameter(1);

            if (command.Length == 0)
            {
                RocketChat.Say(caller, U.Translate("command_p_groups_private", "Your", string.Join(", ", Core.R.Permissions.GetDisplayGroups(caller))));
                RocketChat.Say(caller, U.Translate("command_p_permissions_private", "Your", string.Join(", ", Core.R.Permissions.GetPermissions(caller).ToArray())));
            }
            else if(command.Length == 1 && player != null) {
                RocketChat.Say(caller, U.Translate("command_p_groups_private", player.CharacterName+"s", string.Join(", ", Core.R.Permissions.GetDisplayGroups(player))));
                RocketChat.Say(caller, U.Translate("command_p_permissions_private", player.CharacterName+"s", string.Join(", ", Core.R.Permissions.GetPermissions(player).ToArray())));
            }
            else if (command.Length == 2 && player != null && !String.IsNullOrEmpty(groupName) && player.HasPermission("p.set"))
            {
                if (Core.R.Permissions.SetGroup(player, groupName))
                {
                    RocketChat.Say(caller, U.Translate("command_p_group_assigned", player.CharacterName, groupName));
                }
                else {
                    RocketChat.Say(caller, U.Translate("command_p_group_not_found"));
                }
            }
            else
            {
                RocketChat.Say(caller, U.Translate("command_generic_invalid_parameter"));
                return;
            }

            
         }
    }
}