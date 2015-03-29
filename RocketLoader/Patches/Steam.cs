using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Linq;

namespace Rocket.RocketLoader.Patches
{
    public class Steam : Patch
    {
        private PatchHelper h = new PatchHelper("SDG.Steam");

        public void Apply()
        {
            h.UnlockFieldByType("ClientConnected", "OnClientConnected");
            h.UnlockFieldByType("ClientDisconnected", "OnClientDisconnected");
            h.UnlockFieldByType("ServerHosted", "OnServerHosted");
            h.UnlockFieldByType("ServerShutdown", "OnServerShutdown");
            h.UnlockFieldByType("ServerConnected", "OnServerConnected");
            h.UnlockFieldByType("ServerDisconnected", "OnServerDisconnected");
            h.UnlockFieldByType(typeof(string), "Version");
            h.UnlockFieldByType(typeof(string), "InstanceName", 13);
            h.UnlockFieldByType(typeof(uint), "ServerPort", 1);
            h.UnlockFieldByType(typeof(byte), "MaxPlayers");

            h.UnlockFieldByType(typeof(bool), "PvP",5);

            h.UnlockFieldByType("List<SteamPlayer>", "Players");

            MethodDefinition reject = h.Type.Methods.AsEnumerable().Where(m => m.Parameters.Count == 2 &&
                 m.Parameters[0].ParameterType.Name == "CSteamID" &&
                 m.Parameters[1].ParameterType.Name == "ESteamRejection").FirstOrDefault();
            reject.Name = "Reject";
            reject.IsPublic = true;

            //CheckValid
            MethodDefinition checkValid = RocketLoader.APIAssembly.MainModule.GetType("Rocket.RocketAPI.RocketPermissionManager").Methods.AsEnumerable().Where(m => m.Name == "CheckValid").FirstOrDefault();
            MethodDefinition check = h.Type.Methods.AsEnumerable().Where(m => m.Parameters.Count == 1 && m.Parameters[0].ParameterType.Name == "ValidateAuthTicketResponse_t").FirstOrDefault();
            int i = 0;
            check.Body.GetILProcessor().InsertBefore(check.Body.Instructions[i], Instruction.Create(OpCodes.Call, RocketLoader.UnturnedAssembly.MainModule.Import(checkValid)));
            FieldDefinition field = check.parameters[0].ParameterType.Resolve().Fields.Where(f => f.FieldType.Name =="CSteamID").FirstOrDefault();
            check.Body.GetILProcessor().InsertBefore(check.Body.Instructions[i], Instruction.Create(OpCodes.Ldfld, check.Module.Import(field)));
            check.Body.GetILProcessor().InsertBefore(check.Body.Instructions[i], Instruction.Create(OpCodes.Ldarga_S, check.parameters[0]));
        }
    }
}