using Rocket.RocketAPI;
using SDG;
using System;

namespace Rocket.Commands
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

        public void Execute(Steamworks.CSteamID caller, string command)
        {
            string[] componentsFromSerial = command.Split('/');

            SteamPlayer p = PlayerTool.getSteamPlayer(caller);

            if (componentsFromSerial.Length > 1)
            {
                RocketChatManager.Say(caller, RocketTranslation.Translate("command_generic_invalid_parameter"));
                return;
            }


            Steamworks.CSteamID player = caller;

            string name = "Your";
            if (componentsFromSerial.Length != 0)
            {
                if (componentsFromSerial[0].ToString().ToLower() == "reload" && RocketPermissionManager.CheckPermissions(p, "p.reload"))
                {
                    RocketPermissionManager.ReloadPermissions();
                    RocketChatManager.Say(caller, RocketTranslation.Translate("command_p_reload_private"));
                    return;
                }

                //if (componentsFromSerial[0].ToString().ToLower() == "set" && RocketPermissionManager.CheckPermissions(p, "p.set"))
                //{
                //    if (componentsFromSerial.Length != 5)
                //    {
                //        RocketChatManager.Say(caller.CSteamID, RocketTranslation.Translate("command_generic_invalid_parameter"));
                //    }

                //    SteamPlayer toSetPlayer = PlayerTool.getSteamPlayer("");

                //    RocketPermissionManager.SavePermissions();
                //    RocketChatManager.Say(caller.CSteamID, RocketTranslation.Translate("command_p_set_private", toSetPlayer.SteamPlayerID.CharacterName));
                //    return;
                //}

                ushort id = 0;
                if (!ushort.TryParse(componentsFromSerial[0].ToString(), out id))
                {
                    RocketChatManager.Say(caller, RocketTranslation.Translate("command_generic_invalid_parameter"));
                    return;
                }
                SteamPlayerID otherPlayer = PlayerTool.getPlayer(caller).SteamChannel.SteamPlayer.SteamPlayerID;
                player = otherPlayer.CSteamID;
                name = otherPlayer.CharacterName + "s";
            }

            RocketChatManager.Say(caller, RocketTranslation.Translate("command_p_groups_private", name, String.Join(", ", RocketPermissionManager.GetDisplayGroups(player))));
            RocketChatManager.Say(caller, RocketTranslation.Translate("command_p_permissions_private", name, String.Join(", ", RocketPermissionManager.GetPermissions(player))));
        }
    }
}