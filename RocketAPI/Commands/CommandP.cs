using Rocket.RocketAPI;
using SDG;
using System;

namespace Rocket
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
            string[] componentsFromSerial = Parser.getComponentsFromSerial(command, '/');

            if (componentsFromSerial.Length > 1)
            {
                RocketChatManager.Say(caller.CSteamID, "Invalid Parameter");
                return;
            }

            SteamPlayerID player = caller;

            string name = "Your";
            if (componentsFromSerial.Length != 0)
            {
                ushort id = 0;
                if (!ushort.TryParse(componentsFromSerial[0].ToString(), out id))
                {
                    RocketChatManager.Say(caller.CSteamID, "Invalid Parameter");
                    return;
                }
                player = PlayerTool.getPlayer(caller.CSteamID).SteamChannel.SteamPlayer.SteamPlayerID;
                name = player.CharacterName + "s";
            }

            RocketChatManager.Say(caller.CSteamID, name + " groups are: " + String.Join(", ", RocketPermissionManager.GetDisplayGroups(player.CSteamID)));
            RocketChatManager.Say(caller.CSteamID, name + " permissions are: " + String.Join(", ", RocketPermissionManager.GetPermissions(player.CSteamID)));
        }
    }
}