namespace Rocket.RocketLoader.Patches
{
    public class PlayerInventory : Patch
    {
        private PatchHelper h = new PatchHelper("SDG.PlayerInventory");

        public void Apply()
        {
            h.UnlockFieldByType("InventoryResized", "OnInventoryResized");
            h.UnlockFieldByType("InventoryUpdated", "OnInventoryUpdated");
            h.UnlockFieldByType("InventoryAdded", "OnInventoryAdded");
            h.UnlockFieldByType("InventoryRemoved", "OnInventoryRemoved");
            h.UnlockFieldByType("Items[]", "Items");
        }
    }
}