using Rocket.Logging;
using Rocket.RocketAPI;
using SDG;
using System;

namespace Rocket.Commands
{
    public class CommandI : IRocketCommand
    {
        public bool RunFromConsole
        {
            get { return false; }
        }

        public string Name
        {
            get { return "i"; }
        }

        public string Help
        {
            get { return "Gives yourself an item";}
        }

        public void Execute(RocketPlayer caller, string command)
        {
            string[] componentsFromSerial = Parser.getComponentsFromSerial(command, '/');

            if (componentsFromSerial.Length == 0 || componentsFromSerial.Length > 2)
            {
                RocketChatManager.Say(caller, RocketTranslation.Translate("command_generic_invalid_parameter"));
                return;
            }

            ushort id = 0;
            byte amount = 1;

            string itemString = componentsFromSerial[0].ToString();

            if (!ushort.TryParse(itemString, out id))
            {
                Asset[] assets = SDG.Assets.find(EAssetType.Item);
                foreach (ItemAsset ia in assets)
                {
                    if(ia != null && ia.Name != null && ia.Name.ToLower().Contains(itemString.ToLower())){
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

            Asset a = SDG.Assets.find(EAssetType.Item,id);
            string assetName = ((ItemAsset)a).Name;

            if (componentsFromSerial.Length == 2 && !byte.TryParse(componentsFromSerial[1].ToString(), out amount))
            {
                RocketChatManager.Say(caller, RocketTranslation.Translate("command_generic_invalid_parameter"));
                return;
            }

            if (caller.GiveItem(id, amount))
            {
                Logger.Log(RocketTranslation.Translate("command_i_giving_console",caller.CharacterName, id, amount));
                RocketChatManager.Say(caller, RocketTranslation.Translate("command_i_giving_private", amount, assetName, id));
            }
            else
            {
                RocketChatManager.Say(caller, RocketTranslation.Translate("command_i_giving_failed_private", amount, assetName, id));
            }
        }
    }
}