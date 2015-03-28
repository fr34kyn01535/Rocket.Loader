using Rocket.Components;
using Rocket.Logging;
using SDG;
using Steamworks;
using System;
using UnityEngine;

namespace Rocket.RocketAPI.Events
{
    public enum InventoryGroup { Primary = 0, Secondary = 1, Backpack = 3, Vest = 4, Shirt = 5, Pants = 6, Storage = 7 };

    public sealed partial class RocketPlayerEvents : RocketPlayerComponent
    {
        public delegate void PlayerInventoryUpdated(SDG.Player player, InventoryGroup inventoryGroup, byte inventoryIndex, ItemJar P);
        public static event PlayerInventoryUpdated OnPlayerInventoryUpdated;
        public event PlayerInventoryUpdated OnInventoryUpdated;

        private void onInventoryUpdated(byte E, byte O, ItemJar P)
        {
            try
            {
                if (OnPlayerInventoryUpdated != null) OnPlayerInventoryUpdated(PlayerInstance, (InventoryGroup)Enum.Parse(typeof(InventoryGroup), E.ToString()), O, P);
                if (OnInventoryUpdated != null) OnInventoryUpdated(PlayerInstance, (InventoryGroup)Enum.Parse(typeof(InventoryGroup), E.ToString()), O, P);
            }
            catch (System.Exception ex)
            {
                Logger.Log(ex);
            }
        }

        public delegate void PlayerInventoryResized(SDG.Player player, InventoryGroup inventoryGroup, byte O, byte U);
        public static event PlayerInventoryResized OnPlayerInventoryResized;
        public event PlayerInventoryResized OnInventoryResized;

        private void onInventoryResized(byte E, byte M, byte U)
        {
            try
            {
                if (OnPlayerInventoryResized != null) OnPlayerInventoryResized(PlayerInstance, (InventoryGroup)Enum.Parse(typeof(InventoryGroup), E.ToString()), M, U);
                if (OnInventoryResized != null) OnInventoryResized(PlayerInstance, (InventoryGroup)Enum.Parse(typeof(InventoryGroup), E.ToString()), M, U);
            }
            catch (System.Exception ex)
            {
                Logger.Log(ex);
            }
        }

        public delegate void PlayerInventoryRemoved(SDG.Player player, InventoryGroup inventoryGroup, byte inventoryIndex, ItemJar P);
        public static event PlayerInventoryRemoved OnPlayerInventoryRemoved;
        public event PlayerInventoryRemoved OnInventoryRemoved;

        private void onInventoryRemoved(byte E, byte y, ItemJar f)
        {
            try
            {
                if (OnPlayerInventoryRemoved != null) OnPlayerInventoryRemoved(PlayerInstance, (InventoryGroup)Enum.Parse(typeof(InventoryGroup), E.ToString()), y, f);
                if (OnInventoryRemoved != null) OnInventoryRemoved(PlayerInstance, (InventoryGroup)Enum.Parse(typeof(InventoryGroup), E.ToString()), y, f);
            }
            catch (System.Exception ex)
            {
                Logger.Log(ex);
            }
        }

        public delegate void PlayerInventoryAdded(SDG.Player player, InventoryGroup inventoryGroup, byte inventoryIndex, ItemJar P);
        public static event PlayerInventoryAdded OnPlayerInventoryAdded;
        public event PlayerInventoryAdded OnInventoryAdded;

        private void onInventoryAdded(byte E, byte u, ItemJar J)
        {
            try
            {
                if (OnPlayerInventoryAdded != null) OnPlayerInventoryAdded(PlayerInstance, (InventoryGroup)Enum.Parse(typeof(InventoryGroup), E.ToString()), u, J);
                if (OnInventoryAdded != null) OnInventoryAdded(PlayerInstance, (InventoryGroup)Enum.Parse(typeof(InventoryGroup), E.ToString()), u, J);
            }
            catch (System.Exception ex)
            {
                Logger.Log(ex);
            }
        }
    }
}