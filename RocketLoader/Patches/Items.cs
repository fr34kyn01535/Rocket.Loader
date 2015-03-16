using System;

namespace Rocket.RocketLoader.Patches
{
    public class Items : Patch
    {
        private PatchHelper h = new PatchHelper("SDG.Items");

        public void Apply()
        {
            h.UnlockFieldByType(typeof(byte), "SizeX", 1);
            h.UnlockFieldByType(typeof(byte), "SizeY", 2);

            h.UnlockFieldByType("ItemsResized", "OnItemsResized");
            h.UnlockFieldByType("ItemUpdated", "OnItemUpdated");
            h.UnlockFieldByType("ItemAdded", "OnItemAdded");
            h.UnlockFieldByType("ItemRemoved", "OnItemRemoved");
            h.UnlockFieldByType("ItemDiscarded", "OnItemDiscarded");
            h.UnlockFieldByType("StateUpdated", "OnStateUpdated");
        }
    }
}