using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocket.RocketLoader.Patches
{
    public class PlayerClothing : Patch
    {
        PatchHelper h = new PatchHelper("SDG.PlayerClothing");

        public void Apply()
        {
            h.UnlockFieldByType("ShirtUpdated", "OnShirtUpdated");
            h.UnlockFieldByType("PantsUpdated", "OnPantsUpdated");
            h.UnlockFieldByType("BackpackUpdated", "OnBackpackUpdated");
            h.UnlockFieldByType("VestUpdated", "OnVestUpdated");
        }
    }
}
