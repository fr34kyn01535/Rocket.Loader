using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Linq;

namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.ObjectAsset")]
    internal class ObjectAsset : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType("GameObject", "Model");
        }
    }
}