﻿using Rocket.Logging;
using Rocket.RocketAPI;
using SDG;
using System;

namespace Rocket.Commands
{
    public class CommandI : Command
    {
        public CommandI()
        {
            base.commandName = "i";
            base.commandHelp = "Gives yourself an item";
            base.commandInfo = base.commandName + " - " + base.commandHelp;
        }

        protected override void execute(SteamPlayerID caller, string command)
        {
            if (!RocketCommand.IsPlayer(caller)) return;
            
            string[] componentsFromSerial = Parser.getComponentsFromSerial(command, '/');

            if (componentsFromSerial.Length == 0 || componentsFromSerial.Length > 2)
            {
                RocketChatManager.Say(caller.CSteamID, "Invalid Parameter");
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
                    RocketChatManager.Say(caller.CSteamID, "Invalid Parameter");
                    return;
                }
            }

            Asset a = SDG.Assets.find(EAssetType.Item,id);
            string assetName = ((ItemAsset)a).Name;

            if (componentsFromSerial.Length == 2 && !byte.TryParse(componentsFromSerial[1].ToString(), out amount))
            {
                RocketChatManager.Say(caller.CSteamID, "Invalid Parameter");
                return;
            }

            SDG.Player player = PlayerTool.getPlayer(caller.CSteamID);
            if (ItemTool.tryForceGiveItem(player, id, amount))
            {
                Logger.Log("Giving " + caller.CharacterName +" item " + id + ":" + amount);
                RocketChatManager.Say(caller.CSteamID, "Giving you " + amount + "x " + assetName + " ("+id+")");
            }
            else
            {
                RocketChatManager.Say(caller.CSteamID, "Failed giving you " + amount + "x " + assetName + " (" + id + ")");
            }
        }
    }
}