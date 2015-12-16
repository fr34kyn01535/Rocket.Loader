using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Linq;

namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.Unturned.ChatManager")]
    public class ChatManager : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType("Chat[]", "ChatLog");
            UnlockFieldByType(typeof(string[]), "ChatFilter");
            UnlockFieldByType("ChatManager", "Instance");
            UnlockFieldByType("Chatted", "OnChatted");

            MethodDefinition checkPermissions = GetInterfaceMethod("CheckPermissions");
            MethodDefinition process = GetMethod("process");
            if (process != null)
            {
                process.Body.Instructions[4] = Instruction.Create(OpCodes.Ldstr, "/");
                process.Body.Instructions[8] = Instruction.Create(OpCodes.Call, RocketLoader.UnityAssemblyDefinition.MainModule.ImportReference(checkPermissions));
                process.Body.GetILProcessor().InsertBefore(process.Body.Instructions[8], Instruction.Create(OpCodes.Ldarg_1));
            }

            //MethodDefinition getChatColor = RocketLoader.APIAssemblyDefinition.MainModule.GetType("Rocket.Unturned.Permissions.RocketPermissions").Methods.AsEnumerable().Where(m => m.Name == "GetChatColor").FirstOrDefault();
            //MethodDefinition askChat = GetMethod("askChat");
            //if (askChat != null)
            //{
            //    askChat.Body.GetILProcessor().InsertBefore(askChat.Body.Instructions[132], Instruction.Create(OpCodes.Stloc_2));
            //    askChat.Body.GetILProcessor().InsertBefore(askChat.Body.Instructions[132], Instruction.Create(OpCodes.Call, RocketLoader.UnityAssemblyDefinition.MainModule.Import(getChatColor)));
            //    askChat.Body.GetILProcessor().InsertBefore(askChat.Body.Instructions[132], Instruction.Create(OpCodes.Ldarg_3)); //string message
            //    askChat.Body.GetILProcessor().InsertBefore(askChat.Body.Instructions[132], Instruction.Create(OpCodes.Ldarg_2)); //EChatMode chatMode
            //    askChat.Body.GetILProcessor().InsertBefore(askChat.Body.Instructions[132], Instruction.Create(OpCodes.Ldarg_1)); //CSteamID steamPlayer  
            //}
        }
    }
}