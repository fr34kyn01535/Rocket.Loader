using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Linq;

namespace Rocket.RocketLoader.Patches
{
    public class SteamChannelMethod : Patch
    {
        private PatchHelper h = new PatchHelper("SDG.SteamChannelMethod");

        public void Apply()
        {
            h.UnlockFieldByType("MethodInfo", "MethodInfo");
            h.UnlockFieldByType("Type[]", "Types");
        }
    }
}