using Rocket.API;
using Rocket.Core;
using Rocket.Core.Translations;
using Rocket.Unturned.Logging;
using Rocket.Unturned.Player;
using SDG;
using System;
using System.Linq;
using System.Collections.Generic;
using Rocket.Unturned.Util;

namespace Rocket.Unturned.Commands
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

        public string Syntax
        {
            get { return "<id> [amount]"; }
        }

        public List<string> Aliases
        {
            get { return new List<string>() { "item" }; }
        }

        public void Execute(RocketPlayer caller, string[] command)
        {
            if (command.Length == 0 || command.Length > 2)
            {
                RocketChat.Say(caller, RocketTranslationManager.Translate("command_generic_invalid_parameter"));
                return;
            }

            ushort id = 0;
            byte amount = 1;

            string itemString = command[0].ToString();

            if (!ushort.TryParse(itemString, out id))
            {
                ItemAsset asset = ItemHelper.GetItemAssetByName(itemString.ToLower());
                if (asset != null) id = asset.Id;
                if (String.IsNullOrEmpty(itemString.Trim()) || id == 0)
                {
                    RocketChat.Say(caller, RocketTranslationManager.Translate("command_generic_invalid_parameter"));
                    return;
                }
            }

            Asset a = SDG.Assets.find(EAssetType.Item,id);
            string assetName = ((ItemAsset)a).Name;

            if (command.Length == 2 && !byte.TryParse(command[1].ToString(), out amount))
            {
                RocketChat.Say(caller, RocketTranslationManager.Translate("command_generic_invalid_parameter"));
                return;
            }

            if (caller.GiveItem(id, amount))
            {
                Logger.Log(RocketTranslationManager.Translate("command_i_giving_console",caller.CharacterName, id, amount));
                RocketChat.Say(caller, RocketTranslationManager.Translate("command_i_giving_private", amount, assetName, id));
            }
            else
            {
                RocketChat.Say(caller, RocketTranslationManager.Translate("command_i_giving_failed_private", amount, assetName, id));
            }
        }
    }
}