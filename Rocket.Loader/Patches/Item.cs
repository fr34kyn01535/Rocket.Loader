using System;

namespace Rocket.RocketLoader.Patches
{
    public class Item : Patch
    {
        private PatchHelper h = new PatchHelper("SDG.Item");

        public void Apply()
        {
            h.UnlockFieldByType(typeof(Byte), "Amount", 0);
            h.UnlockFieldByType(typeof(Byte), "Durability", 1);
            h.UnlockFieldByType(typeof(Byte[]), "Metadata");
            h.UnlockFieldByType(typeof(ushort), "ItemID");
        }
    }
}