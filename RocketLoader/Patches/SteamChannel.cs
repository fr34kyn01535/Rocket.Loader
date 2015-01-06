using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocket.RocketLoader.Patches
{
    public class SteamChannel : Patch
    {
        PatchHelper h = new PatchHelper("SDG.SteamChannel");

        public void Apply()
        {
            h.UnlockFieldByType("SteamPlayer", "SteamPlayer");
        }
    }
}
