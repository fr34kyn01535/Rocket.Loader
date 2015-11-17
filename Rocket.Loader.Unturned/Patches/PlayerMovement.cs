//using Mono.Cecil;
//using Mono.Cecil.Cil;
//using Mono.Collections.Generic;
//using System.Linq;

//namespace Rocket.RocketLoader.Unturned.Patches
//{
//    [Class("SDG.Unturned.PlayerMovement")]
//    public class PlayerMovement : Patch
//    {
//        public override void Apply()
//        {



//            Collection<MethodDefinition> s =  RocketLoader.APIAssemblyDefinition.MainModule.GetType("Rocket.Unturned.Test").Methods;


//            FieldDefinition EnableRocketVanishMode = new FieldDefinition("EnableRocketVanishMode", FieldAttributes.Public, Type.Module.ImportReference(typeof(bool)));
//            Type.Fields.Add(EnableRocketVanishMode);

//            MethodDefinition constructor = GetMethod(".ctor");
//            constructor.Body.GetILProcessor().InsertAfter(constructor.Body.Instructions[0], Instruction.Create(OpCodes.Ldarg_0));
//            constructor.Body.GetILProcessor().InsertAfter(constructor.Body.Instructions[0], Instruction.Create(OpCodes.Stfld, EnableRocketVanishMode));
//            constructor.Body.GetILProcessor().InsertAfter(constructor.Body.Instructions[0], Instruction.Create(OpCodes.Ldc_I4_0));

//            MethodDefinition def = new MethodDefinition("Test", MethodAttributes.Public, Type.Module.ImportReference(typeof(void)));

//            //ldarg.0
//            def.Body.GetILProcessor().Create(OpCodes.Ldarg_0);

//            Type.Methods.Add(def);

            
//        }
//    }
//}