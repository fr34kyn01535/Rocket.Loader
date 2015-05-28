using System.Collections.Generic;
namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.LevelNodes")]
    public class LevelNodes : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType("List<Node>", "Nodes");
        }
    }
}