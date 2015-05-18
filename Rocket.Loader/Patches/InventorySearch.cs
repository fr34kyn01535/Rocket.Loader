namespace Rocket.RocketLoader.Patches
{
    public class InventorySearch : IPatch
    {
        private PatchHelper h = new PatchHelper("SDG.InventorySearch");

        public void Apply()
        {
            h.UnlockFieldByType("ItemJar", "ItemJar");
            h.UnlockFieldByType(typeof(byte), "InventoryGroup");
        }
    }
}