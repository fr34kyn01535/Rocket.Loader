using System;

namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.Unturned.ItemManager")]
    public class ItemManager : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType("ItemRegion[,]", "ItemRegions");
            UnlockFieldByType("ItemManager", "Instance");
        }
    }
}