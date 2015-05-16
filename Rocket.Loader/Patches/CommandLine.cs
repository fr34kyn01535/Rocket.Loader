using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Linq;

namespace Rocket.RocketLoader.Patches
{
    internal class CommandLine : Patch
    {
        private PatchHelper h = new PatchHelper("SDG.CommandLine");

        public void Apply()
        {
            MethodDefinition splash = RocketLoader.APIAssembly.MainModule.GetType("Rocket.Unturned.Implementation").Methods.AsEnumerable().Where(m => m.Name == "Splash").FirstOrDefault();
            MethodDefinition getCommands = h.GetMethod("getCommands");
            getCommands.Body.GetILProcessor().InsertBefore(getCommands.Body.Instructions[0], Instruction.Create(OpCodes.Call, RocketLoader.UnturnedAssembly.MainModule.Import(splash)));
        }
    }
}