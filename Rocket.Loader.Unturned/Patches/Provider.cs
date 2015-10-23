using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Linq;

namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.Unturned.Provider")]
    public class Provider : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType(typeof(string), "Version");

            UnlockFieldByType(typeof(ushort), "ServerPort");

            UnlockFieldByType(typeof(byte), "MaxPlayers");

            UnlockFieldByType(typeof(bool), "PvP", 7);
            
            UnlockFieldByType("List<SteamPlayer>", "Players");

            MethodDefinition reject = Type.Methods.AsEnumerable().Where(m => m.Parameters.Count == 2 &&
                 m.Parameters[0].ParameterType.Name == "CSteamID" &&
                 m.Parameters[1].ParameterType.Name == "ESteamRejection").FirstOrDefault();
            reject.Name = "Reject";
            reject.IsPublic = true;
            
            //CheckValid
            MethodDefinition checkValid = GetInterfaceMethod("CheckValid");
            MethodDefinition check = Type.Methods.AsEnumerable().Where(m => m.Parameters.Count == 1 && m.Parameters[0].ParameterType.Name == "ValidateAuthTicketResponse_t").FirstOrDefault();

            FieldDefinition field = check.Parameters[0].ParameterType.Resolve().Fields.Where(f => f.FieldType.Name == "CSteamID").FirstOrDefault();

            int i = 3;

            check.Body.GetILProcessor().InsertBefore(check.Body.Instructions[i], Instruction.Create(OpCodes.Ldarg_0));
            check.Body.GetILProcessor().InsertBefore(check.Body.Instructions[i+1], Instruction.Create(OpCodes.Call, RocketLoader.UnityAssemblyDefinition.MainModule.Import(checkValid)));
            check.Body.GetILProcessor().InsertBefore(check.Body.Instructions[i+2], Instruction.Create(OpCodes.Ret));
            check.Body.GetILProcessor().InsertBefore(check.Body.Instructions[i+2], Instruction.Create(OpCodes.Brtrue_S, check.Body.Instructions[i+3]));
        }
    }
}