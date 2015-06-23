using System.Collections.Generic;
namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.Unturned.LevelNodes")]
    public class LevelNodes : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType("List<Node>", "Nodes");
        }
    }
}