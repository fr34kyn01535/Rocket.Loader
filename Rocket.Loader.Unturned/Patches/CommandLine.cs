using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Linq;

namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.Unturned.CommandLine")]
    internal class CommandLine : Patch
    {
        public override void Apply()
        {
            MethodDefinition splash = GetInterfaceMethod("Splash");
            MethodDefinition getCommands = GetMethod("getCommands");
            getCommands.Body.GetILProcessor().InsertBefore(getCommands.Body.Instructions[0], Instruction.Create(OpCodes.Call, RocketLoader.UnityAssemblyDefinition.MainModule.ImportReference(splash)));
        }
    }
}