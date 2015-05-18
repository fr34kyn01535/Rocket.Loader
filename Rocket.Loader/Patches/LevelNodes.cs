using System.Collections.Generic;
namespace Rocket.RocketLoader.Patches
{
    public class LevelNodes : IPatch
    {
        private PatchHelper h = new PatchHelper("SDG.LevelNodes");

        public void Apply()
        {
            h.UnlockFieldByType("List<Node>", "Nodes");
        }
    }
}