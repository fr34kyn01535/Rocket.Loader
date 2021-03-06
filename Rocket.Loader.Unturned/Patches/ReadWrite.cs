﻿//using Mono.Cecil;
//using Mono.Cecil.Cil;
//using System.IO;
//using System.Linq;
//using System.Security.Cryptography;

//namespace Rocket.RocketLoader.Unturned.Patches
//{
//    [Class("SDG.Unturned.ReadWrite")]
//    public class ReadWrite : Patch
//    {
//        public static byte[] combine(params byte[][] r)
//        {
//            byte[] array = new byte[r.Length * 20];

//            for (int i = 0; i < r.Length; i++)
//                r[i].CopyTo(array, (int)(i * 20));

//            return SHA1(array);
//        }

//        static SHA1CryptoServiceProvider sha = new SHA1CryptoServiceProvider();

//        public static byte[] SHA1(byte[] U)
//        {
//            return sha.ComputeHash(U);
//        }

//        public override void Apply()
//        {
//            byte[] windows, windows_firstpass, mac, mac_firstpass, linux, linux_firstpass;


//#if LINUX
//            using (FileStream filestream = new FileStream("Assembly-CSharp.dll", FileMode.Open, FileAccess.Read, FileShare.Read))
//            {
//                linux = new byte[filestream.Length];
//            }

//            using (FileStream filestream = new FileStream("Other-Assembly-CSharp.dll", FileMode.Open, FileAccess.Read, FileShare.Read))
//            {
//                windows = new byte[filestream.Length];
//            }

//            using (FileStream filestream = new FileStream("Other2-Assembly-CSharp.dll", FileMode.Open, FileAccess.Read, FileShare.Read))
//            {
//                mac = new byte[filestream.Length];
//            }


//            using (FileStream filestream = new FileStream("Assembly-CSharp-firstpass.dll", FileMode.Open, FileAccess.Read, FileShare.Read))
//            {
//                linux_firstpass = new byte[filestream.Length];
//            }

//            using (FileStream filestream = new FileStream("Other-Assembly-CSharp-firstpass.dll", FileMode.Open, FileAccess.Read, FileShare.Read))
//            {
//                 windows_firstpass = new byte[filestream.Length];
//            }

//            using (FileStream filestream = new FileStream("Other2-Assembly-CSharp-firstpass.dll", FileMode.Open, FileAccess.Read, FileShare.Read))
//            {
//                mac_firstpass = new byte[filestream.Length];
//            }
//#else
//            using (FileStream filestream = new FileStream("Assembly-CSharp.dll", FileMode.Open, FileAccess.Read, FileShare.Read))
//            {
//                windows = new byte[filestream.Length];
//            }

//            using (FileStream filestream = new FileStream("Other-Assembly-CSharp.dll", FileMode.Open, FileAccess.Read, FileShare.Read))
//            {
//                mac = new byte[filestream.Length];
//            }

//            using (FileStream filestream = new FileStream("Other2-Assembly-CSharp.dll", FileMode.Open, FileAccess.Read, FileShare.Read))
//            {
//                linux = new byte[filestream.Length];
//            }


//            using (FileStream filestream = new FileStream("Assembly-CSharp-firstpass.dll", FileMode.Open, FileAccess.Read, FileShare.Read))
//            {
//                windows_firstpass = new byte[filestream.Length];
//            }

//            using (FileStream filestream = new FileStream("Other-Assembly-CSharp-firstpass.dll", FileMode.Open, FileAccess.Read, FileShare.Read))
//            {
//                mac_firstpass = new byte[filestream.Length];
//            }

//            using (FileStream filestream = new FileStream("Other2-Assembly-CSharp-firstpass.dll", FileMode.Open, FileAccess.Read, FileShare.Read))
//            {
//                linux_firstpass = new byte[filestream.Length];
//            }
//#endif
//            byte[] combined = combine(new byte[][] { SHA1(windows), SHA1(mac), SHA1(linux), SHA1(windows_firstpass), SHA1(mac_firstpass), SHA1(linux_firstpass) });

//            MethodDefinition getAssemblyHash = GetMethod("getAssemblyHash");
//            getAssemblyHash.Body.Instructions.Clear();

//            MethodDefinition newgetAssemblyHash = RocketLoader.PatchAssemblyDefinition.MainModule.GetType("Rocket.RocketLoader.Unturned.Patches.ReadWrite").Methods.AsEnumerable().Where(m => m.Name == "getAssemblyHash").FirstOrDefault();

//            TypeReference byteType = AssemblyDefinition.ReadAssembly("mscorlib.dll").MainModule.GetType("System.Byte");

//            TypeReference fieldType = RocketLoader.UnityAssemblyDefinition.MainModule.Import(byteType);
//            bool flag1 = false, flag2 = false;

//            //TODO fix this shit

//            foreach (Instruction i in newgetAssemblyHash.Body.Instructions)
//            {
//                if (i.OpCode.Name.ToLower().Equals("stloc.0"))
//                    flag1 = true;
//                else if (!i.OpCode.Name.ToLower().Equals("ldloc.0"))
//                    flag1 = false;

//                if (flag1 && i.OpCode.Name.ToLower().Equals("ldloc.0"))
//                    flag2 = true;

//                if (!i.OpCode.Name.ToLower().Equals("newarr"))
//                    getAssemblyHash.Body.Instructions.Add(i);
//                else
//                    getAssemblyHash.Body.Instructions.Add(Instruction.Create(OpCodes.Newarr, fieldType));

//                if (flag1 && flag2)
//                {
//                    for (int i2 = 0; i2 < combined.Length; i2++)
//                    {
//                        getAssemblyHash.Body.Instructions.Add(Instruction.Create(OpCodes.Ldloc_0));
//                        Instruction inst1 = Instruction.Create(OpCodes.Ldc_I4, (int)i2);
//                        Instruction inst2 = Instruction.Create(OpCodes.Ldc_I4, (int)combined[i2]);
//                        getAssemblyHash.Body.Instructions.Add(inst1);
//                        getAssemblyHash.Body.Instructions.Add(inst2);
//                        getAssemblyHash.Body.Instructions.Add(Instruction.Create(OpCodes.Stelem_I1));
//                    }
//                }
//            }
//        }

//        private static System.Byte[] getAssemblyHash()
//        {
//            byte[] b = new System.Byte[20];
//            return b;
//        }
//    }
//}