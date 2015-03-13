using Rocket.RocketAPI;
using SDG;
using System;

namespace Rocket.Commands
{
    public class CommandP : Command
    {
        public CommandP()
        {
            base.commandName = "p";
            base.commandHelp = "Lists permissions";
            base.commandInfo = base.commandName + " - " + base.commandHelp;
        }

        protected override void execute(SteamPlayerID caller, string command)
        {
            if (!RocketCommand.IsPlayer(caller)) return;

            string[] componentsFromSerial = Parser.getComponentsFromSerial(command, '/');

            SteamPlayer p = PlayerTool.getSteamPlayer(caller.CSteamID);

            if (componentsFromSerial.Length > 1)
            {
                RocketChatManager.Say(caller.CSteamID, RocketTranslation.Translate("command_generic_invalid_parameter"));
                return;
            }


            SteamPlayerID player = caller;

            string name = "Your";
            if (componentsFromSerial.Length != 0)
            {

                if (componentsFromSerial[0].ToString().ToLower() == "reload" && RocketPermissionManager.CheckPermissions(p, "p.reload"))
                {
                    RocketPermissionManager.ReloadPermissions();
                    RocketChatManager.Say(caller.CSteamID, RocketTranslation.Translate("command_p_reload_private"));
                    return;
                }

                ushort id = 0;
                if (!ushort.TryParse(componentsFromSerial[0].ToString(), out id))
                {
                    RocketChatManager.Say(caller.CSteamID, RocketTranslation.Translate("command_generic_invalid_parameter"));
                    return;
                }
                player = PlayerTool.getPlayer(caller.CSteamID).SteamChannel.SteamPlayer.SteamPlayerID;
                name = player.CharacterName + "s";
            }

            RocketChatManager.Say(caller.CSteamID, RocketTranslation.Translate("command_p_groups_private", name, String.Join(", ", RocketPermissionManager.GetDisplayGroups(player.CSteamID))));
            RocketChatManager.Say(caller.CSteamID, RocketTranslation.Translate("command_p_permissions_private", name, String.Join(", ", RocketPermissionManager.GetPermissions(player.CSteamID))));
        }
    }
}