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

            MethodDefinition InventoryItemClicked = RocketLoader.APIAssembly.MainModule.GetType("Rocket.RocketAPI.Events").Methods.AsEnumerable().Where(m => m.Name == "InventoryItemClicked").FirstOrDefault();
            MethodDefinition askDragItem = h.GetMethod("askDragItem");


           /* askDragItem.Body.GetILProcessor().InsertBefore(askDragItem.Body.Instructions[0], Instruction.Create(OpCodes.Call, RocketLoader.UnturnedAssembly.MainModule.Import(InventoryItemClicked)));

            //Add all parameters from the called method
            for (int i = askDragItem.Parameters.Count - 1; i >= 0; i--)
            {
                askDragItem.Body.GetILProcessor().InsertBefore(askDragItem.Body.Instructions[0], Instruction.Create(OpCodes.Ldarg, askDragItem.Parameters[i]));
            }
         */
        }
    }
}
