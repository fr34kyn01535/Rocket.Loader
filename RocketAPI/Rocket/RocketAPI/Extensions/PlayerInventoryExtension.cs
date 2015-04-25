using Rocket.Logging;
using SDG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rocket.RocketAPI
{
    public static class PlayerInventoryExtension
    {
        public static bool Clear(this PlayerInventory inventory)
        {
            try
            {
                Player p = inventory.transform.GetComponent<Player>();
                p.Equipment.dequip();
                for (byte a = 7; (a >= 0 && a <= 7); a--)
                {
                    byte count = inventory.getItemCount(a);
                    if (count > 0)
                    {
                        for (byte b = (byte)(count - 1); (b >= 0 && b <= (byte)(count - 1)); b--)
                        {
                            inventory.removeItem(a, b);
                        }
                    }
                }
                // Now reset the Clothing class.
                if (PlayerSavedata.fileExists(p.SteamChannel.SteamPlayer.SteamPlayerID, "/Player/Clothing.dat"))
                    // Delete the file
                    PlayerSavedata.deleteFile(p.SteamChannel.SteamPlayer.SteamPlayerID, "/Player/Clothing.dat");
                // Now reset the Clothing class.
                p.Clothing.load();
                // And send to the player the update to their clothing.
                p.SteamChannel.send("tellClothing", ESteamCall.ALL, ESteamPacket.UPDATE_UDP_BUFFER, new object[]
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
                p.SteamChannel.send("tellSlot", ESteamCall.ALL, ESteamPacket.UPDATE_TCP_BUFFER, new object[]
		        {
			        0,
			        0,
			        new byte[0]
		        });
                p.SteamChannel.send("tellSlot", ESteamCall.ALL, ESteamPacket.UPDATE_TCP_BUFFER, new object[]
		        {
			        1,
			        0,
			        new byte[0]
		        });
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                return false;
            }
            return true;
        }
    }
}
