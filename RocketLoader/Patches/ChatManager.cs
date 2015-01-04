using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocket.Patches
{
    public class ChatManager : Patch
    {
        public void Apply()
        {
            TypeDefinition t = RocketLoader.UnturnedAssembly.MainModule.GetType("SDG.ChatManager");

            PatchHelper.UnlockByType(t, "Chat[]", "chatLog");



            TypeDefinition loaderType = RocketLoader.APIAssembly.MainModule.GetType("Rocket.RocketPermissionManager");
             MethodDefinition checkPermissions = RocketLoader.GetMethod(loaderType, "CheckPermissions");


            MethodDefinition m = t.Methods.Where(p => p.Name == "process").FirstOrDefault();
            if (m != null)
            {
                m.Body.Instructions[4] = Instruction.Create(OpCodes.Ldstr,"/");
                m.Body.Instructions[8] = Instruction.Create(OpCodes.Call, RocketLoader.UnturnedAssembly.MainModule.Import(checkPermissions));
                m.Body.GetILProcessor().InsertBefore(m.Body.Instructions[8], Instruction.Create(OpCodes.Ldarg_1)); 
            }

        }
    }
}
