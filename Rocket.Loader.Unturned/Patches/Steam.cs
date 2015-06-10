using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Linq;

namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.Steam")]
    public class Steam : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType("ClientConnected", "OnClientConnected");
            UnlockFieldByType("ClientDisconnected", "OnClientDisconnected");
            UnlockFieldByType("ServerHosted", "OnServerHosted");
            UnlockFieldByType("ServerShutdown", "OnServerShutdown");
            UnlockFieldByType("ServerConnected", "OnServerConnected");
            UnlockFieldByType("ServerDisconnected", "OnServerDisconnected");
            UnlockFieldByType(typeof(string), "Version");
            UnlockFieldByType(typeof(string), "InstanceName", 13);
            UnlockFieldByType(typeof(uint), "ServerPort", 1);
            UnlockFieldByType(typeof(byte), "MaxPlayers");

            UnlockFieldByType(typeof(bool), "PvP", 4);
            UnlockFieldByType(typeof(bool), "IsServer", 9);

            UnlockFieldByType("ConsoleInput", "ConsoleInput");
            UnlockFieldByType("ConsoleOutput", "ConsoleOutput");
            
            UnlockFieldByType("List<SteamPlayer>", "Players");
            MethodDefinition reject = Type.Methods.AsEnumerable().Where(m => m.Parameters.Count == 2 &&
                 m.Parameters[0].ParameterType.Name == "CSteamID" &&
                 m.Parameters[1].ParameterType.Name == "ESteamRejection").FirstOrDefault();
            reject.Name = "Reject";
            reject.IsPublic = true;

#if !LINUX
            MethodDefinition log = Type.Methods.AsEnumerable().Where(m => m.Parameters.Count == 2 &&
                 m.Parameters[0].ParameterType.Name == "Object" &&
                 m.Parameters[1].ParameterType.Name == "ConsoleColor").FirstOrDefault();
            log.Name = "Log";
            log.IsPublic = true;

            MethodDefinition externalLog = RocketLoader.APIAssemblyDefinition.MainModule.GetType("Rocket.Unturned.Logging.Logger").Methods.AsEnumerable().Where(m => m.Name == "ExternalLog").FirstOrDefault();

            log.Body.GetILProcessor().InsertBefore(log.Body.Instructions[0], Instruction.Create(OpCodes.Call, RocketLoader.UnityAssemblyDefinition.MainModule.Import(externalLog)));
            log.Body.GetILProcessor().InsertBefore(log.Body.Instructions[0], Instruction.Create(OpCodes.Ldarg_1));
            log.Body.GetILProcessor().InsertBefore(log.Body.Instructions[0], Instruction.Create(OpCodes.Ldarg_0));
#endif
             

            //CheckValid
            MethodDefinition checkValid = RocketLoader.APIAssemblyDefinition.MainModule.GetType("Rocket.Unturned.Permissions.RocketPermissions").Methods.AsEnumerable().Where(m => m.Name == "CheckValid").FirstOrDefault();
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