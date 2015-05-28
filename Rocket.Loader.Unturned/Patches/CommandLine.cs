using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Linq;

namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.CommandLine")]
    internal class CommandLine : Patch
    {
        public override void Apply()
        {
            MethodDefinition splash = RocketLoader.APIAssemblyDefinition.MainModule.GetType("Rocket.Unturned.Implementation").Methods.AsEnumerable().Where(m => m.Name == "Splash").FirstOrDefault();
            MethodDefinition getCommands = GetMethod("getCommands");
            getCommands.Body.GetILProcessor().InsertBefore(getCommands.Body.Instructions[0], Instruction.Create(OpCodes.Call, RocketLoader.UnityAssemblyDefinition.MainModule.Import(splash)));
        }
    }
}