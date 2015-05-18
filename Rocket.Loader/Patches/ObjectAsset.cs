using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Linq;

namespace Rocket.RocketLoader.Patches
{
    internal class ObjectAsset : IPatch
    {
        private PatchHelper h = new PatchHelper("SDG.ObjectAsset");

        public void Apply()
        {
            h.UnlockFieldByType("GameObject", "Model");
        }
    }
}