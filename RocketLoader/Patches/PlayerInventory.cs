using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocket.RocketLoader.Patches
{
    public class PlayerInventory : Patch
    {
        PatchHelper h = new PatchHelper("SDG.PlayerInventory");

        public void Apply()
        {
            h.UnlockFieldByType("InventoryResized", "OnInventoryResized");
            h.UnlockFieldByType("InventoryUpdated", "OnInventoryUpdated");
            h.UnlockFieldByType("InventoryAdded", "OnInventoryAdded");
            h.UnlockFieldByType("InventoryRemoved", "OnInventoryRemoved");
            h.UnlockFieldByType("Items[]", "Items");
        }
    }
}
