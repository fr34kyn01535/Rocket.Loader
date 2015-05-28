using System;

namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.Item")]
    public class Item : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType(typeof(Byte), "Amount", 0);
            UnlockFieldByType(typeof(Byte), "Durability", 1);
            UnlockFieldByType(typeof(Byte[]), "Metadata");
            UnlockFieldByType(typeof(ushort), "ItemID");
        }
    }
}