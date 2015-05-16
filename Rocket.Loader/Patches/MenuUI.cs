using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Linq;

namespace Rocket.RocketLoader.Patches
{
    public class MenuUI : Patch
    {
        private PatchHelper h = new PatchHelper("SDG.MenuUI");

        public void Apply()
        {
            h.RemoveMethod("Update");
            h.RemoveMethod("OnGUI");

           MethodDefinition start = h.GetMethod("Start");
           MethodDefinition Instantiate = RocketLoader.APIAssembly.MainModule.GetType("Rocket.Unturned.RocketUI").Methods.AsEnumerable().Where(m => m.Name == "Instantiate").FirstOrDefault();
            start.Body.GetILProcessor().InsertBefore(start.Body.Instructions[0],Instruction.Create(OpCodes.Call, RocketLoader.UnturnedAssembly.MainModule.Import(Instantiate)));
        }
    }
}