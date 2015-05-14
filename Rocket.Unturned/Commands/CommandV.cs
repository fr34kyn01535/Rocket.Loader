using Rocket.Logging;
using Rocket.RocketAPI;
using SDG;
using System;

namespace Rocket.Unturned.Commands
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

        public void Execute(RocketPlayer caller, string[] command)
        {
            if (command.Length != 1)
            {
                RocketChatManager.Say(caller, RocketTranslation.Translate("command_generic_invalid_parameter"));
                return;
            }

            ushort id = 0;

            string itemString = command[0].ToString();

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

            if (VehicleTool.giveVehicle(caller.Player, id))
            {
                Logger.Log(RocketTranslation.Translate("command_v_giving_console", caller.CharacterName, id));
                RocketChatManager.Say(caller, RocketTranslation.Translate("command_v_giving_private", assetName, id));
            }
            else
            {
                RocketChatManager.Say(caller, RocketTranslation.Translate("command_v_giving_failed_private", assetName, id));
            }
        }
    }
}