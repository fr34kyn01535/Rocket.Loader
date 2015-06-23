using System;

namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.Unturned.Asset")]
    public class Asset : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType(typeof(ushort), "Id");
        }
    }
}