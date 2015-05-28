namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.PlayerInventory")]
    public class PlayerInventory : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType("InventoryResized", "OnInventoryResized");
            UnlockFieldByType("InventoryUpdated", "OnInventoryUpdated");
            UnlockFieldByType("InventoryAdded", "OnInventoryAdded");
            UnlockFieldByType("InventoryRemoved", "OnInventoryRemoved");
            UnlockFieldByType("Items[]", "Items");
        }
    }
}