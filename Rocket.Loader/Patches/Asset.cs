using System;

namespace Rocket.RocketLoader.Patches
{
    public class Asset : Patch
    {
        private PatchHelper h = new PatchHelper("SDG.Asset");

        public void Apply()
        {
            h.UnlockFieldByType(typeof(ushort), "Id");
        }
    }
}