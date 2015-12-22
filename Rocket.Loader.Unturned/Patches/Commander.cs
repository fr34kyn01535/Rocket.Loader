using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;
using System.Linq;

namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.Unturned.Commander")]
    public class Commander : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType("List<Command>", "Commands", 0);
            
            //Execute
            MethodDefinition Execute = GetInterfaceMethod("Execute");
            MethodDefinition execute = Type.Methods.AsEnumerable().Where(m => m.Name == "execute").FirstOrDefault();
            
            Collection<Instruction> instructions = execute.Body.Instructions;

            //0 LDARG0
            //1 LDARG1
            //2 CALL 
            //3 BRTRUE_S > 6
            //4 LDC I4 1 (TRUE)
            //5 RET
            //6 NOP

            instructions.Insert(0, Instruction.Create(OpCodes.Ldarg_0));
            instructions.Insert(1, Instruction.Create(OpCodes.Ldarg_1));
            instructions.Insert(2, Instruction.Create(OpCodes.Call, RocketLoader.UnityAssemblyDefinition.MainModule.ImportReference(Execute)));
            Instruction nop = Instruction.Create(OpCodes.Nop);
            instructions.Insert(3, Instruction.Create(OpCodes.Brfalse_S, nop));
            instructions.Insert(4, Instruction.Create(OpCodes.Ldc_I4_1));
            instructions.Insert(5, Instruction.Create(OpCodes.Ret));
            instructions.Insert(6, nop);
        }

    }
}