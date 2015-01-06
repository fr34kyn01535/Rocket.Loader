using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocket.RocketLoader.Patches
{
    public class ItemJar : Patch
    {
        PatchHelper h = new PatchHelper("SDG.ItemJar");

        public void Apply()
        {
            h.UnlockFieldByType("Item", "Item");
            h.UnlockFieldByType(typeof(byte), "ItemID", 2);
            
        }
    }
}
