namespace Rocket.RocketLoader.Patches
{
    public class ItemJar : IPatch
    {
        private PatchHelper h = new PatchHelper("SDG.ItemJar");

        public void Apply()
        {
            h.UnlockFieldByType("Item", "Item");
            h.UnlockFieldByType(typeof(byte), "PositionX", 0);
            h.UnlockFieldByType(typeof(byte), "PositionY", 1);
            h.UnlockFieldByType(typeof(byte), "SizeX", 2);
            h.UnlockFieldByType(typeof(byte), "SizeY", 3);
        }
    }
}