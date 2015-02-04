using Rocket.Components;
using Rocket.Logging;
using SDG;
using Steamworks;
using UnityEngine;

namespace Rocket.RocketAPI.Events
{
    public partial class RocketPlayerEvents : RocketPlayerComponent
    {
        public delegate void PlayerInventoryUpdated(SDG.Player player, byte inventoryGroup, byte inventoryIndex, ItemJar P);
        public static event PlayerInventoryUpdated OnPlayerInventoryUpdated;
        public event PlayerInventoryUpdated OnInventoryUpdated;

        private void onInventoryUpdated(byte E, byte O, ItemJar P)
        {
            try
            {
                if (OnPlayerInventoryUpdated != null) OnPlayerInventoryUpdated(PlayerInstance, E, O, P);
                if (OnInventoryUpdated != null) OnInventoryUpdated(PlayerInstance, E, O, P);
            }
            catch (System.Exception ex)
            {
                Logger.Log(ex);
            }
        }

        public delegate void PlayerInventoryResized(SDG.Player player, byte I, byte O, byte U);
        public static event PlayerInventoryResized OnPlayerInventoryResized;
        public event PlayerInventoryResized OnInventoryResized;

        private void onInventoryResized(byte I, byte M, byte U)
        {
            try
            {
                if (OnPlayerInventoryResized != null) OnPlayerInventoryResized(PlayerInstance, I, M, U);
                if (OnInventoryResized != null) OnInventoryResized(PlayerInstance, I, M, U);
            }
            catch (System.Exception ex)
            {
                Logger.Log(ex);
            }
        }

        public delegate void PlayerInventoryRemoved(SDG.Player player, byte inventoryGroup, byte inventoryIndex, ItemJar P);
        public static event PlayerInventoryRemoved OnPlayerInventoryRemoved;
        public event PlayerInventoryRemoved OnInventoryRemoved;

        private void onInventoryRemoved(byte V, byte y, ItemJar f)
        {
            try
            {
                if (OnPlayerInventoryRemoved != null) OnPlayerInventoryRemoved(PlayerInstance, V, y, f);
                if (OnInventoryRemoved != null) OnInventoryRemoved(PlayerInstance, V, y, f);
            }
            catch (System.Exception ex)
            {
                Logger.Log(ex);
            }
        }

        public delegate void PlayerInventoryAdded(SDG.Player player, byte inventoryGroup, byte inventoryIndex, ItemJar P);
        public static event PlayerInventoryAdded OnPlayerInventoryAdded;
        public event PlayerInventoryAdded OnInventoryAdded;

        private void onInventoryAdded(byte Q, byte u, ItemJar J)
        {
            try
            {
                if (OnPlayerInventoryAdded != null) OnPlayerInventoryAdded(PlayerInstance, Q, u, J);
                if (OnInventoryAdded != null) OnInventoryAdded(PlayerInstance, Q, u, J);
            }
            catch (System.Exception ex)
            {
                Logger.Log(ex);
            }
        }
    }
}