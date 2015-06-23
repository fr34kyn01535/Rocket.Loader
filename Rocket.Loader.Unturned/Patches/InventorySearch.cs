namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.Unturned.InventorySearch")]
    public class InventorySearch : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType("ItemJar", "ItemJar");
            UnlockFieldByType(typeof(byte), "InventoryGroup");
        }
    }
}