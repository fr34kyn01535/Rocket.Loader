using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Linq;

namespace Rocket.RocketLoader.Hurtworld.Patches
{
    [Class("GameManager")]
    internal class GameManager : Patch
    {
        public override void Apply()
        {
            MethodDefinition splash = GetInterfaceMethod("Splash", "Hurtworld");
            MethodDefinition start = GetMethod("InvalidateServerConfig");
            start.Body.GetILProcessor().InsertBefore(start.Body.Instructions[0], Instruction.Create(OpCodes.Call, RocketLoader.UnityAssemblyDefinition.MainModule.ImportReference(splash)));
        }
    }
}