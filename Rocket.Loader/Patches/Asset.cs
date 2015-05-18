using System;

namespace Rocket.RocketLoader.Patches
{
    public class Asset : IPatch
    {
        private PatchHelper h = new PatchHelper("SDG.Asset");

        public void Apply()
        {
            h.UnlockFieldByType(typeof(ushort), "Id");
        }
    }
}