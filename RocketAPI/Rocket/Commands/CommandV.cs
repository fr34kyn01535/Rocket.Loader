using Rocket.Logging;
using Rocket.RocketAPI;
using SDG;
using System;

namespace Rocket.Commands
{
    public class CommandV : Command
    {
        public CommandV()
        {
            base.commandName = "v";
            base.commandHelp = "Gives yourself an vehicle";
            base.commandInfo = base.commandName + " - " + base.commandHelp;
        }

        protected override void execute(SteamPlayerID caller, string command)
        {
            if (!RocketCommand.IsPlayer(caller)) return;

            string[] componentsFromSerial = Parser.getComponentsFromSerial(command, '/');

            if (componentsFromSerial.Length == 0 || componentsFromSerial.Length > 1)
            {
                RocketChatManager.Say(caller.CSteamID, "Invalid Parameter");
                return;
            }

            ushort id = 0;

            if (!ushort.TryParse(componentsFromSerial[0].ToString(), out id))
            {
                RocketChatManager.Say(caller.CSteamID, "Invalid Parameter");
                return;
            }

            SDG.Player player = PlayerTool.getPlayer(caller.CSteamID);
            if (VehicleTool.giveVehicle(player, id))
            {
                Logger.Log("Giving " + caller.CharacterName + " vehicle " + id);
               RocketChatManager.Say(caller.CSteamID, "Giving you vehicle " + id);
            }
            else
            {
               RocketChatManager.Say(caller.CSteamID, "Failed giving vehicle " + id);
            }
        }
    }
}