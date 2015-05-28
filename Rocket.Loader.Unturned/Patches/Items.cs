using System;

namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.Items")]
    public class Items : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType(typeof(byte), "SizeX", 1);
            UnlockFieldByType(typeof(byte), "SizeY", 2);

            UnlockFieldByType("ItemsResized", "OnItemsResized");
            UnlockFieldByType("ItemUpdated", "OnItemUpdated");
            UnlockFieldByType("ItemAdded", "OnItemAdded");
            UnlockFieldByType("ItemRemoved", "OnItemRemoved");
            UnlockFieldByType("ItemDiscarded", "OnItemDiscarded");
            UnlockFieldByType("StateUpdated", "OnStateUpdated");
        }
    }
}