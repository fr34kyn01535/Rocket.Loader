using Mono.Cecil;
using Mono.Cecil.Cil;

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
            MethodDefinition Instantiate = GetInterfaceMethod("InstantiateUI"); 
            start.Body.GetILProcessor().InsertBefore(start.Body.Instructions[0],Instruction.Create(OpCodes.Call, RocketLoader.UnityAssemblyDefinition.MainModule.Import(Instantiate)));
        }
    }
}