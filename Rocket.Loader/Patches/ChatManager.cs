using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Linq;

namespace Rocket.RocketLoader.Patches
{
    public class ChatManager : IPatch
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

#if INTERNAL
            MethodDefinition getChatColor = RocketLoader.APIAssembly.MainModule.GetType("Rocket.Unturned.Permissions.RocketPermissions").Methods.AsEnumerable().Where(m => m.Name == "GetChatColor").FirstOrDefault();
            MethodDefinition askChat = h.GetMethod("askChat");
            if (askChat != null)
            {
                askChat.Body.GetILProcessor().InsertBefore(askChat.Body.Instructions[132], Instruction.Create(OpCodes.Stloc_2));
                askChat.Body.GetILProcessor().InsertBefore(askChat.Body.Instructions[132], Instruction.Create(OpCodes.Call, RocketLoader.UnturnedAssembly.MainModule.Import(getChatColor)));
                askChat.Body.GetILProcessor().InsertBefore(askChat.Body.Instructions[132], Instruction.Create(OpCodes.Ldarg_3)); //string message
                askChat.Body.GetILProcessor().InsertBefore(askChat.Body.Instructions[132], Instruction.Create(OpCodes.Ldarg_2)); //EChatMode chatMode
                askChat.Body.GetILProcessor().InsertBefore(askChat.Body.Instructions[132], Instruction.Create(OpCodes.Ldarg_1)); //CSteamID steamPlayer  
            }
#endif
        }
    }
}