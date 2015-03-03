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

            string itemString = componentsFromSerial[0].ToString();

            if (!ushort.TryParse(itemString, out id))
            {
                Asset[] assets = SDG.Assets.find(EAssetType.Vehicle);
                foreach (VehicleAsset ia in assets)
                {
                    if (ia != null && ia.Name != null && ia.Name.ToLower().Contains(itemString.ToLower()))
                    {
                        id = ia.Id;
                        break;
                    }
                }
                if (String.IsNullOrEmpty(itemString.Trim()) || id == 0)
                {
                    RocketChatManager.Say(caller.CSteamID, "Invalid Parameter");
                    return;
                }
            }

            Asset a = SDG.Assets.find(EAssetType.Vehicle, id);
            string assetName = ((VehicleAsset)a).Name;


            SDG.Player player = PlayerTool.getPlayer(caller.CSteamID);
            if (VehicleTool.giveVehicle(player, id))
            {
                Logger.Log("Giving " + caller.CharacterName + " vehicle " + id);
                RocketChatManager.Say(caller.CSteamID, "Giving you a " + assetName + " (" + id + ")");
            }
            else
            {
                RocketChatManager.Say(caller.CSteamID, "Failed giving you a " + assetName + " (" + id + ")");
            }
        }
    }
}