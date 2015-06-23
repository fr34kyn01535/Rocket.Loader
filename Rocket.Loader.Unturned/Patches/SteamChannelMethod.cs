using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Linq;

namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.Unturned.SteamChannelMethod")]
    public class SteamChannelMethod : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType("MethodInfo", "MethodInfo");
            UnlockFieldByType("Type[]", "Types");
        }
    }
}