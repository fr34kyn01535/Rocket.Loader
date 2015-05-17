using Rocket.API;
using Rocket.Core;
using Rocket.Core.Permissions;
using Rocket.Core.Translations;
using Rocket.Unturned.Logging;
using Rocket.Unturned.Player;
using SDG;
using System;

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

        public void Execute(RocketPlayer caller, string[] command)
        {
            if (command.Length > 1)
            {
                RocketChat.Say(caller, RocketTranslationManager.Translate("command_generic_invalid_parameter"));
                return;
            }

            RocketChat.Say(caller, RocketTranslationManager.Translate("command_p_groups_private", "Your", String.Join(", ", RocketPermissionsManager.GetDisplayGroups(caller.CSteamID.ToString()))));
            RocketChat.Say(caller, RocketTranslationManager.Translate("command_p_permissions_private", "Your", String.Join(", ", RocketPermissionsManager.GetPermissions(caller.CSteamID.ToString()).ToArray())));
        }
    }
}