using System.Collections.Generic;
namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.Unturned.NodeLocation")]
    public class NodeLocation : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType(typeof(string), "Name");
        }
    }
}