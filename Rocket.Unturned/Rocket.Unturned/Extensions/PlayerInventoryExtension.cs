using Rocket.Unturned.Logging;
using SDG;
using System;

namespace Rocket.Unturned
{
    public static class PlayerInventoryExtension
    {
        public static bool Clear(this PlayerInventory inventory)
        {
            try
            {
                SDG.Player p = inventory.transform.GetComponent<SDG.Player>();
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
                    (byte)0,
                    (byte)0,
                    (byte)0,
                    (byte)0,
                    (byte)0,
                    (byte)0,
                    (byte)0,
                    (byte)0,
                    (byte)0,
                    (byte)0,
                    (byte)0,
                    (byte)0,
                    (byte)0,
                    (byte)0
                });
                p.SteamChannel.send("tellSlot", ESteamCall.ALL, ESteamPacket.UPDATE_TCP_BUFFER, new object[]
		        {
			        (byte)0,
			        (byte)0,
			        new byte[0]
		        });
                p.SteamChannel.send("tellSlot", ESteamCall.ALL, ESteamPacket.UPDATE_TCP_BUFFER, new object[]
		        {
			        (byte)1,
			        (byte)0,
			        new byte[0]
		        });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return false;
            }
            return true;
        }
    }
}
