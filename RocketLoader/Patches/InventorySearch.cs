namespace Rocket.RocketLoader.Patches
{
    public class InventorySearch : Patch
    {
        private PatchHelper h = new PatchHelper("SDG.InventorySearch");

        public void Apply()
        {
            h.UnlockFieldByType("ItemJar", "ItemJar");
        }
    }
}