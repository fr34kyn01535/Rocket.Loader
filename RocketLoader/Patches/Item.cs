using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocket.RocketLoader.Patches
{
    public class Item : Patch
    {
        PatchHelper h = new PatchHelper("SDG.Item");

        public void Apply()
        {
            h.UnlockFieldByType(typeof(Byte), "Amount",0);
            h.UnlockFieldByType(typeof(Byte), "Durability", 1);
            h.UnlockFieldByType(typeof(Byte[]), "Metadata");
            h.UnlockFieldByType(typeof(ushort), "ItemID");
        }
    }
}
