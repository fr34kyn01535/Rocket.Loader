namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.Unturned.ItemData")]
    public class ItemData : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType("Item", "Item");
            UnlockFieldByType("Vector3", "Position");
            UnlockFieldByType(typeof(bool), "IsDropped");
            UnlockFieldByType(typeof(float), "LastDropped");
        }
    }
}