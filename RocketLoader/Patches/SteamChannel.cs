using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocket.RocketLoader.Patches
{
    public class SteamChannel : Patch
    {
        PatchHelper h = new PatchHelper("SDG.SteamChannel");

        public void Apply()
        {
            h.UnlockFieldByType("SteamPlayer", "SteamPlayer");

            MethodDefinition sendInstructions = RocketLoader.APIAssembly.MainModule.GetType("Rocket.RocketAPI.RocketEvents").Methods.AsEnumerable().Where(m => m.Name == "send").FirstOrDefault();
            
            MethodDefinition send = h.Type.Methods.AsEnumerable().Where(m => m.Parameters.Count == 4 &&
                 m.Parameters[0].ParameterType.Name == "String" &&
                 m.Parameters[1].ParameterType.Name == "ESteamCall" &&
                 m.Parameters[2].ParameterType.Name == "ESteamPacket" &&
                 m.Parameters[3].ParameterType.Name == "Object[]").FirstOrDefault();


            send.Body.GetILProcessor().InsertBefore(send.Body.Instructions[0], Instruction.Create(OpCodes.Call, RocketLoader.UnturnedAssembly.MainModule.Import(sendInstructions)));

            send.Body.GetILProcessor().InsertBefore(send.Body.Instructions[0], Instruction.Create(OpCodes.Ldarg_S, sendInstructions.Parameters[3]));
            send.Body.GetILProcessor().InsertBefore(send.Body.Instructions[0], Instruction.Create(OpCodes.Ldarg_3));
            send.Body.GetILProcessor().InsertBefore(send.Body.Instructions[0], Instruction.Create(OpCodes.Ldarg_2));
            send.Body.GetILProcessor().InsertBefore(send.Body.Instructions[0], Instruction.Create(OpCodes.Ldarg_1));

            FieldDefinition steamPlayer = h.Type.Fields.AsEnumerable().Where(m => m.Name == "SteamPlayer").FirstOrDefault();
            send.Body.GetILProcessor().InsertBefore(send.Body.Instructions[0], Instruction.Create(OpCodes.Ldfld, steamPlayer));
            send.Body.GetILProcessor().InsertBefore(send.Body.Instructions[0], Instruction.Create(OpCodes.Ldarg_0));
            
        }
    }
}
