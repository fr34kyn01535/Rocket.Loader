using System.Collections.Generic;
namespace Rocket.RocketLoader.Patches
{
    public class NodeLocation : IPatch
    {
        private PatchHelper h = new PatchHelper("SDG.NodeLocation");

        public void Apply()
        {
            h.UnlockFieldByType(typeof(string), "Name");
        }
    }
}