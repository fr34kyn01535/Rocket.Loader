using Rocket.Logging;
using Rocket.RocketAPI;
using SDG;
using System;

namespace Rocket.Commands
{
    public class CommandV : IRocketCommand
    {
        public bool RunFromConsole
        {
            get { return false; }
        }

        public string Name
        {
            get { return "v"; }
        }

        public string Help
        {
            get { return "Gives yourself an vehicle";}
        }

        public void Execute(Steamworks.CSteamID caller, string command)
        {
            string[] componentsFromSerial = command.Split('/');

            if (componentsFromSerial.Length == 0 || componentsFromSerial.Length > 1)
            {
                RocketChatManager.Say(caller, RocketTranslation.Translate("command_generic_invalid_parameter"));
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
                    RocketChatManager.Say(caller, RocketTranslation.Translate("command_generic_invalid_parameter"));
                    return;
                }
            }

            Asset a = SDG.Assets.find(EAssetType.Vehicle, id);
            string assetName = ((VehicleAsset)a).Name;


            SDG.Player player = PlayerTool.getPlayer(caller);
            if (VehicleTool.giveVehicle(player, id))
            {
                Logger.Log(RocketTranslation.Translate("command_v_giving_console", player.SteamChannel.SteamPlayer.SteamPlayerID.CharacterName, id));
                RocketChatManager.Say(caller, RocketTranslation.Translate("command_v_giving_private", assetName, id));
            }
            else
            {
                RocketChatManager.Say(caller, RocketTranslation.Translate("command_v_giving_failed_private", assetName, id));
            }
        }
    }
}