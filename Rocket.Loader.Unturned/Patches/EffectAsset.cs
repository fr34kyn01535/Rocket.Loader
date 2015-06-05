﻿using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Linq;

namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.EffectAsset")]
    public class EffectAsset : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType("GameObject", "GameObject");
            UnlockFieldByType(typeof(bool), "Gore");
            UnlockFieldByType("GameObject[]", "Splatter");
            UnlockFieldByType(typeof(byte), "Splatters");
            UnlockFieldByType(typeof(float), "Lifetime");

            //RECEIVE
            MethodDefinition receiveInstruction = RocketLoader.APIAssemblyDefinition.MainModule.GetType("Rocket.Unturned.RocketEffectManager").Methods.AsEnumerable().Where(m => m.Name == "RegisterRocketEffect").FirstOrDefault();
            MethodDefinition receive = Type.Methods.AsEnumerable().Where(m => m.Name == ".ctor").FirstOrDefault();
            int i = receive.Body.Instructions.Count - 3;
            receive.Body.GetILProcessor().InsertBefore(receive.Body.Instructions[i], Instruction.Create(OpCodes.Call, RocketLoader.UnityAssemblyDefinition.MainModule.Import(receiveInstruction)));
            receive.Body.GetILProcessor().InsertBefore(receive.Body.Instructions[i], Instruction.Create(OpCodes.Ldarg_3));
            receive.Body.GetILProcessor().InsertBefore(receive.Body.Instructions[i], Instruction.Create(OpCodes.Ldarg_2));
            receive.Body.GetILProcessor().InsertBefore(receive.Body.Instructions[i], Instruction.Create(OpCodes.Ldarg_1));

        }
    }
}