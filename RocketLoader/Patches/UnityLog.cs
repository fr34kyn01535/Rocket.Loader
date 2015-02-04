using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocket.RocketLoader.Patches
{
    internal class UnityLog : Patch
    {

        public void Apply()
        {
            try
            {
                IEnumerable<MethodDefinition> methods = RocketLoader.UnityAssembly.MainModule.GetType("UnityEngine.Debug").Methods.AsEnumerable();
                IEnumerable<MethodDefinition> hookMethods = RocketLoader.APIAssembly.MainModule.GetType("Rocket.Logging.Logger").Methods.AsEnumerable();

                MethodDefinition log = methods.Where(m => m.Name == "Log").ToArray()[0];
                MethodDefinition log2 = methods.Where(m => m.Name == "Log").ToArray()[1];
                MethodDefinition logError = methods.Where(m => m.Name == "LogError").ToArray()[0];
                MethodDefinition logError2 = methods.Where(m => m.Name == "LogError").ToArray()[1];
                MethodDefinition logEx = methods.Where(m => m.Name == "LogException").ToArray()[0];
                MethodDefinition logEx2 = methods.Where(m => m.Name == "LogException").ToArray()[1];
                MethodDefinition logWarn = methods.Where(m => m.Name == "LogWarning").ToArray()[0];
                MethodDefinition logWarn2 = methods.Where(m => m.Name == "LogWarning").ToArray()[1];

                MethodDefinition hookLog = hookMethods.Where(m => m.Name == "ExternalLog").ToArray()[0];
                MethodDefinition hookLog2 = hookMethods.Where(m => m.Name == "ExternalLog").ToArray()[1];
                MethodDefinition hookLogError = hookMethods.Where(m => m.Name == "ExternalLogError").ToArray()[0];
                MethodDefinition hookLogError2 = hookMethods.Where(m => m.Name == "ExternalLogError").ToArray()[1];
                MethodDefinition hookLogEx = hookMethods.Where(m => m.Name == "ExternalLogException").ToArray()[0];
                MethodDefinition hookLogEx2 = hookMethods.Where(m => m.Name == "ExternalLogException").ToArray()[1];
                MethodDefinition hookLogWarn = hookMethods.Where(m => m.Name == "ExternalLogWarning").ToArray()[0];
                MethodDefinition hookLogWarn2 = hookMethods.Where(m => m.Name == "ExternalLogWarning").ToArray()[1];

                log.Body.GetILProcessor().InsertBefore(log.Body.Instructions[0], Instruction.Create(OpCodes.Call, RocketLoader.UnityAssembly.MainModule.Import(hookLog)));
                log.Body.GetILProcessor().InsertBefore(log.Body.Instructions[0], Instruction.Create(OpCodes.Ldarg_0));

                log2.Body.GetILProcessor().InsertBefore(log2.Body.Instructions[0], Instruction.Create(OpCodes.Call, RocketLoader.UnityAssembly.MainModule.Import(hookLog2)));
                log2.Body.GetILProcessor().InsertBefore(log2.Body.Instructions[0], Instruction.Create(OpCodes.Ldarg_1));
                log2.Body.GetILProcessor().InsertBefore(log2.Body.Instructions[0], Instruction.Create(OpCodes.Ldarg_0));

                logError.Body.GetILProcessor().InsertBefore(logError.Body.Instructions[0], Instruction.Create(OpCodes.Call, RocketLoader.UnityAssembly.MainModule.Import(hookLogError)));
                logError.Body.GetILProcessor().InsertBefore(logError.Body.Instructions[0], Instruction.Create(OpCodes.Ldarg_0));

                logError2.Body.GetILProcessor().InsertBefore(logError2.Body.Instructions[0], Instruction.Create(OpCodes.Call, RocketLoader.UnityAssembly.MainModule.Import(hookLogError2)));
                logError2.Body.GetILProcessor().InsertBefore(logError2.Body.Instructions[0], Instruction.Create(OpCodes.Ldarg_1));
                logError2.Body.GetILProcessor().InsertBefore(logError2.Body.Instructions[0], Instruction.Create(OpCodes.Ldarg_0));

                logEx.Body.GetILProcessor().InsertBefore(logEx.Body.Instructions[0], Instruction.Create(OpCodes.Call, RocketLoader.UnityAssembly.MainModule.Import(hookLogEx)));
                logEx.Body.GetILProcessor().InsertBefore(logEx.Body.Instructions[0], Instruction.Create(OpCodes.Ldarg_0));

                logEx2.Body.GetILProcessor().InsertBefore(logEx2.Body.Instructions[0], Instruction.Create(OpCodes.Call, RocketLoader.UnityAssembly.MainModule.Import(hookLogEx2)));
                logEx2.Body.GetILProcessor().InsertBefore(logEx2.Body.Instructions[0], Instruction.Create(OpCodes.Ldarg_1));
                logEx2.Body.GetILProcessor().InsertBefore(logEx2.Body.Instructions[0], Instruction.Create(OpCodes.Ldarg_0));

                logWarn.Body.GetILProcessor().InsertBefore(logWarn.Body.Instructions[0], Instruction.Create(OpCodes.Call, RocketLoader.UnityAssembly.MainModule.Import(hookLogWarn)));
                logWarn.Body.GetILProcessor().InsertBefore(logWarn.Body.Instructions[0], Instruction.Create(OpCodes.Ldarg_0));

                logWarn2.Body.GetILProcessor().InsertBefore(logWarn2.Body.Instructions[0], Instruction.Create(OpCodes.Call, RocketLoader.UnityAssembly.MainModule.Import(hookLogWarn2)));
                logWarn2.Body.GetILProcessor().InsertBefore(logWarn2.Body.Instructions[0], Instruction.Create(OpCodes.Ldarg_1));
                logWarn2.Body.GetILProcessor().InsertBefore(logWarn2.Body.Instructions[0], Instruction.Create(OpCodes.Ldarg_0));


            }
            catch (Exception e)
            {

                Console.WriteLine(e);

            }


        }

    }
}
