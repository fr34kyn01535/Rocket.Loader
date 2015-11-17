using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Linq;

namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.Unturned.SteamChannel")]
    public class SteamChannel : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType("SteamPlayer", "SteamPlayer");
            UnlockFieldByType("SteamChannelMethod[]", "Methods");

            //SEND
            MethodDefinition sendInstruction = GetInterfaceMethod("TriggerSend");

            MethodDefinition send = Type.Methods.AsEnumerable().Where(m => m.Parameters.Count == 4 &&
                 m.Parameters[0].ParameterType.Name == "String" &&
                 m.Parameters[1].ParameterType.Name == "ESteamCall" &&
                 m.Parameters[2].ParameterType.Name == "ESteamPacket" &&
                 m.Parameters[3].ParameterType.Name == "Object[]").FirstOrDefault();

            send.Body.GetILProcessor().InsertBefore(send.Body.Instructions[0], Instruction.Create(OpCodes.Call, RocketLoader.UnityAssemblyDefinition.MainModule.ImportReference(sendInstruction)));

            send.Body.GetILProcessor().InsertBefore(send.Body.Instructions[0], Instruction.Create(OpCodes.Ldarg_S, sendInstruction.Parameters[3]));
            send.Body.GetILProcessor().InsertBefore(send.Body.Instructions[0], Instruction.Create(OpCodes.Ldarg_3));
            send.Body.GetILProcessor().InsertBefore(send.Body.Instructions[0], Instruction.Create(OpCodes.Ldarg_2));
            send.Body.GetILProcessor().InsertBefore(send.Body.Instructions[0], Instruction.Create(OpCodes.Ldarg_1));

            FieldDefinition steamPlayer = Type.Fields.AsEnumerable().Where(m => m.Name == "SteamPlayer").FirstOrDefault();
            send.Body.GetILProcessor().InsertBefore(send.Body.Instructions[0], Instruction.Create(OpCodes.Ldfld, steamPlayer));
            send.Body.GetILProcessor().InsertBefore(send.Body.Instructions[0], Instruction.Create(OpCodes.Ldarg_0));

            //RECEIVE
            MethodDefinition receiveInstruction = GetInterfaceMethod("TriggerReceive");
            MethodDefinition receive = Type.Methods.AsEnumerable().Where(m => m.Name=="receive").FirstOrDefault();
            int i = 30;
            receive.Body.GetILProcessor().InsertBefore(receive.Body.Instructions[i], Instruction.Create(OpCodes.Call, RocketLoader.UnityAssemblyDefinition.MainModule.ImportReference(receiveInstruction)));
            receive.Body.GetILProcessor().InsertBefore(receive.Body.Instructions[i], Instruction.Create(OpCodes.Ldarg_3));
            receive.Body.GetILProcessor().InsertBefore(receive.Body.Instructions[i], Instruction.Create(OpCodes.Ldarg_2));
            receive.Body.GetILProcessor().InsertBefore(receive.Body.Instructions[i], Instruction.Create(OpCodes.Ldarg_1));
            receive.Body.GetILProcessor().InsertBefore(receive.Body.Instructions[i], Instruction.Create(OpCodes.Ldarg_0));

        }
    }
}