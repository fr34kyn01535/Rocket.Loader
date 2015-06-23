using System.Collections.Generic;
namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.Unturned.Node")]
    public class Node : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType("Vector3", "Position");
            UnlockFieldByType("ENodeType", "NodeType");
        }
    }
}