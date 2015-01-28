using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Linq;

namespace Rocket.RocketLoader.Patches
{
    public class Managers : Patch
    {
        private PatchHelper h = new PatchHelper("SDG.Managers");

        public void Apply()
        {
            MethodDefinition launch = RocketLoader.APIAssembly.MainModule.GetType("Rocket.RocketLauncher").Methods.AsEnumerable().Where(m => m.Name == "Launch").FirstOrDefault();
            MethodDefinition awake = h.GetMethod("Awake");

            awake.Body.GetILProcessor().InsertBefore(awake.Body.Instructions[0], Instruction.Create(OpCodes.Call, RocketLoader.UnturnedAssembly.MainModule.Import(launch)));
        }
    }
}