using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Linq;

namespace Rocket.RocketLoader.Patches
{
    public class ChatManager : Patch
    {
        private PatchHelper h = new PatchHelper("SDG.ChatManager");

        public void Apply()
        {
            h.UnlockFieldByType("Chat[]", "ChatLog");
            h.UnlockFieldByType(typeof(string[]), "ChatFilter");
            h.UnlockFieldByType("ChatManager", "Instance");

            MethodDefinition checkPermissions = RocketLoader.APIAssembly.MainModule.GetType("Rocket.Unturned.Permissions.RocketPermissions").Methods.AsEnumerable().Where(m => m.Name == "CheckPermissions").FirstOrDefault();
            MethodDefinition process = h.GetMethod("process");
            if (process != null)
            {
                process.Body.Instructions[4] = Instruction.Create(OpCodes.Ldstr, "/");
                process.Body.Instructions[8] = Instruction.Create(OpCodes.Call, RocketLoader.UnturnedAssembly.MainModule.Import(checkPermissions));
                process.Body.GetILProcessor().InsertBefore(process.Body.Instructions[8], Instruction.Create(OpCodes.Ldarg_1));
            }
        }
    }
}