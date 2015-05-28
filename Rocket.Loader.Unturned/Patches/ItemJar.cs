﻿namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.ItemJar")]
    public class ItemJar : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType("Item", "Item");
            UnlockFieldByType(typeof(byte), "PositionX", 0);
            UnlockFieldByType(typeof(byte), "PositionY", 1);
            UnlockFieldByType(typeof(byte), "SizeX", 2);
            UnlockFieldByType(typeof(byte), "SizeY", 3);
        }
    }
}