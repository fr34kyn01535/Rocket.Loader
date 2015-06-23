using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Linq;

namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.Unturned.MenuUI")]
    public class MenuUI : Patch
    {
        public override void Apply()
        {
            RemoveMethod("Update");
            RemoveMethod("OnGUI");

           MethodDefinition start = GetMethod("Start");
           MethodDefinition Instantiate = RocketLoader.APIAssemblyDefinition.MainModule.GetType("Rocket.Unturned.RocketUI").Methods.AsEnumerable().Where(m => m.Name == "Instantiate").FirstOrDefault();
            start.Body.GetILProcessor().InsertBefore(start.Body.Instructions[0],Instruction.Create(OpCodes.Call, RocketLoader.UnityAssemblyDefinition.MainModule.Import(Instantiate)));
        }
    }
}