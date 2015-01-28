namespace Rocket.RocketLoader.Patches
{
    public class ItemJar : Patch
    {
        private PatchHelper h = new PatchHelper("SDG.ItemJar");

        public void Apply()
        {
            h.UnlockFieldByType("Item", "Item");
            h.UnlockFieldByType(typeof(byte), "PositionX", 0);
            h.UnlockFieldByType(typeof(byte), "PositionY", 1);
        }
    }
}