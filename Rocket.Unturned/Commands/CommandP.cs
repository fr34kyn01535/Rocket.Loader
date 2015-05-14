using Rocket.RocketAPI;
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
            foreach (string p in caller.Permissions)
            {
                Rocket.Logging.Logger.Log("P:" + p);
            }

            if (command.Length > 1)
            {
                RocketChatManager.Say(caller, RocketTranslation.Translate("command_generic_invalid_parameter"));
                return;
            }

            RocketChatManager.Say(caller, RocketTranslation.Translate("command_p_groups_private", "Your", String.Join(", ", RocketPermissionManager.GetDisplayGroups(caller.CSteamID))));
            RocketChatManager.Say(caller, RocketTranslation.Translate("command_p_permissions_private", "Your", String.Join(", ", RocketPermissionManager.GetPermissions(caller.CSteamID).ToArray())));
        }
    }
}