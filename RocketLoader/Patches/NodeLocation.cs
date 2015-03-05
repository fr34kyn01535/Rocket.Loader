using System.Collections.Generic;
namespace Rocket.RocketLoader.Patches
{
    public class NodeLocation : Patch
    {
        private PatchHelper h = new PatchHelper("SDG.NodeLocation");

        public void Apply()
        {
            h.UnlockFieldByType(typeof(string), "Name");
        }
    }
}