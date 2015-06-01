using Rocket.API;
using Rocket.Core;
using Rocket.Core.Translations;
using Rocket.Unturned.Logging;
using Rocket.Unturned.Player;
using SDG;
using System;
using System.Collections.Generic;

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

        public string Syntax
        {
            get { return "<id>"; }
        }

        public List<string> Aliases
        {
            get { return new List<string>(); }
        }

        public void Execute(RocketPlayer caller, string[] command)
        {
            if (command.Length != 1)
            {
                RocketChat.Say(caller, RocketTranslationManager.Translate("command_generic_invalid_parameter"));
                return;
            }

            ushort? id = command.GetUInt16Parameter(0);

            if (!id.HasValue)
            {
                string itemString = command.GetStringParameter(0);

                if (itemString == null)
                {
                    RocketChat.Say(caller, RocketTranslationManager.Translate("command_generic_invalid_parameter"));
                    return;
                }

                Asset[] assets = SDG.Assets.find(EAssetType.Vehicle);
                foreach (VehicleAsset ia in assets)
                {
                    if (ia != null && ia.Name != null && ia.Name.ToLower().Contains(itemString.ToLower()))
                    {
                        id = ia.Id;
                        break;
                    }
                } 
            }

            Asset a = SDG.Assets.find(EAssetType.Vehicle, id.Value);
            string assetName = ((VehicleAsset)a).Name;

            if (VehicleTool.giveVehicle(caller.Player, id.Value))
            {
                Logger.Log(RocketTranslationManager.Translate("command_v_giving_console", caller.CharacterName, id));
                RocketChat.Say(caller, RocketTranslationManager.Translate("command_v_giving_private", assetName, id));
            }
            else
            {
                RocketChat.Say(caller, RocketTranslationManager.Translate("command_v_giving_failed_private", assetName, id));
            }
        }
    }
}