using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocket.RocketLoader.Patches
{
    class CommandLine : Patch
    {
        PatchHelper h = new PatchHelper("SDG.CommandLine");

        public void Apply()
        {
            MethodDefinition splash = RocketLoader.APIAssembly.MainModule.GetType("Rocket.RocketLauncher").Methods.AsEnumerable().Where(m => m.Name == "Splash").FirstOrDefault();
            MethodDefinition getCommands = h.GetMethod("getCommands");

            getCommands.Body.GetILProcessor().InsertBefore(getCommands.Body.Instructions[0], Instruction.Create(OpCodes.Call, RocketLoader.UnturnedAssembly.MainModule.Import(splash)));
        }
    }
}
