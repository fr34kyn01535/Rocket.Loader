using Rocket.Logging;
using Rocket.RocketAPI;
using SDG;
using System;

namespace Rocket.Commands
{
    public class CommandCi : IRocketCommand
    {
        public bool RunFromConsole
        {
            get { return false; }
        }

        public string Name
        {
            get { return "ci"; }
        }

        public string Help
        {
            get { return "Clears your inventory"; }
        }

        public void Execute(RocketPlayer caller, string command)
        {
            caller.Player.Equipment.dequip();
            for (byte a = 7; (a >= 0 && a <= 7); a--)
            {
                byte count = caller.Inventory.getItemCount(a);
                if (count > 0)
                {
                    for (byte b = (byte)(count - 1); (b >= 0 && b <= (byte)(count - 1)); b--)
                    {
                        caller.Inventory.removeItem(a, b);
                    }
                }
            }
            // Now reset the Clothing class.
            if (PlayerSavedata.fileExists(caller.Player.SteamChannel.SteamPlayer.SteamPlayerID, "/Player/Clothing.dat"))
            {
                // Delete the file
                PlayerSavedata.deleteFile(caller.Player.SteamChannel.SteamPlayer.SteamPlayerID, "/Player/Clothing.dat");
                // Now reset the Clothing class.
                caller.Player.Clothing.load();
                // And send to the player the update to their clothing.
                caller.Player.SteamChannel.send("tellClothing", ESteamCall.ALL, ESteamPacket.UPDATE_UDP_BUFFER, new object[]
                            {
                                    0,
                                    0,
                                    0,
                                    0,
                                    0,
                                    0,
                                    0,
                                    0,
                                    0,
                                    0,
                                    0,
                                    0,
                                    0,
                                    0
                            });
            }
            else
            {
                Logger.Log("Something went wrong removing " + caller.CharacterName + "'s clothing!");
            }

            RocketChatManager.Say(caller, RocketTranslation.Translate("command_clear_private"));

        }
    }
}