using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Linq;

namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.Unturned.CommandWindow")]
    public class CommandWindow : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType("ConsoleInput", "ConsoleInput");
            UnlockFieldByType("ConsoleOutput", "ConsoleOutput");
#if !LINUX
            MethodDefinition log = Type.Methods.AsEnumerable().Where(m => m.Parameters.Count == 2 &&
                 m.Parameters[0].ParameterType.Name == "Object" &&
                 m.Parameters[1].ParameterType.Name == "ConsoleColor").FirstOrDefault();
            log.Name = "Log";
            log.IsPublic = true;

            MethodDefinition externalLog = GetInterfaceMethod("ExternalLog");

            log.Body.GetILProcessor().InsertBefore(log.Body.Instructions[0], Instruction.Create(OpCodes.Call, RocketLoader.UnityAssemblyDefinition.MainModule.Import(externalLog)));
            log.Body.GetILProcessor().InsertBefore(log.Body.Instructions[0], Instruction.Create(OpCodes.Ldarg_1));
            log.Body.GetILProcessor().InsertBefore(log.Body.Instructions[0], Instruction.Create(OpCodes.Ldarg_0));
#endif
        }
    }
}