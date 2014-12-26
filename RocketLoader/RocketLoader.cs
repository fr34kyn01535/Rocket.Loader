using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Threading;
using System.Diagnostics;
using System.Reflection;

namespace Rocket
{
    public class RocketLoader
    {
        public static AssemblyDefinition UnturnedAssembly, LoaderAssembly;

        static void Main(string[] args)
        {
            Console.WriteLine("RocketLoader Version " + Assembly.GetExecutingAssembly().GetName().Version.ToString());
           
            try
            {
                UnturnedAssembly = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
                LoaderAssembly = AssemblyDefinition.ReadAssembly("RocketAPI.dll");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadKey();
                Environment.Exit(1);
            }

            attachBootstrap();
            fixHash();

            var patches = from t in Assembly.GetExecutingAssembly().GetTypes()
                          where t.GetInterfaces().Contains(typeof(Patch)) && t.GetConstructor(Type.EmptyTypes) != null
                          select Activator.CreateInstance(t) as Patch;

            foreach (var patch in patches)
            {
                patch.Apply();
            }

            removePlayButton(); 

            Console.WriteLine("Writing to file...");
            UnturnedAssembly.Write("Assembly-CSharp.dll");
        }

        private static void removePlayButton()
        {
            Console.WriteLine("Removing Play Button");
            TypeDefinition td = UnturnedAssembly.MainModule.GetType("SDG.MenuPlayUI");
            MethodDefinition ctor = GetMethod(td, ".ctor");
            ctor.Body.Instructions.Clear();
        }

        private static void attachBootstrap()
        {
            TypeDefinition loaderType = LoaderAssembly.MainModule.GetType("Rocket.RocketAPI.Bootstrap");
            MethodDefinition initBootstrap = GetMethod(loaderType, "LaunchRocket");

            TypeDefinition unturnedType = UnturnedAssembly.MainModule.GetType("SDG.Managers");
            MethodDefinition awake = GetMethod(unturnedType, "Awake");

            if (awake.Body.Instructions[0].ToString().Contains("LaunchRocket")) {
                Console.WriteLine("Already patched!");
                Environment.Exit(0);
            }

            awake.Body.GetILProcessor().InsertBefore(awake.Body.Instructions[0], Instruction.Create(OpCodes.Call, UnturnedAssembly.MainModule.Import(initBootstrap)));
            Console.WriteLine("Attached Bootstrap");
        }

        private static void fixHash()
        {
            byte[] unturned, unturned_firstpass, other, other_firstpass;
            using (FileStream filestream = new FileStream("Assembly-CSharp.dll", FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                unturned = new byte[filestream.Length];
            }

            using (FileStream filestream = new FileStream("Assembly-CSharp-firstpass.dll", FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                unturned_firstpass = new byte[filestream.Length];
            }

            using (FileStream filestream = new FileStream("Other-Assembly-CSharp.dll", FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                other = new byte[filestream.Length];
            }

            using (FileStream filestream = new FileStream("Other-Assembly-CSharp-firstpass.dll", FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                other_firstpass = new byte[filestream.Length];
            }

            byte[] combined = combine(new byte[][] { SHA1(unturned), SHA1(other), SHA1(unturned_firstpass), SHA1(other_firstpass) });


            MethodDefinition unturnedMethod = GetMethod(UnturnedAssembly.MainModule.GetType("SDG.ReadWrite"), "getAssemblyHash");
            MethodDefinition overrideMethod = GetMethod(LoaderAssembly.MainModule.GetType("Rocket.RocketAPI.Bootstrap"), "getAssemblyHash");

            TypeReference byteType = AssemblyDefinition.ReadAssembly("mscorlib.dll").MainModule.GetType("System.Byte");

            Console.WriteLine("Overriding hash check");

            unturnedMethod.Body.Instructions.Clear();

            Console.WriteLine("Cleared hash func, injecting new code");

            TypeReference fieldType = UnturnedAssembly.MainModule.Import(byteType);
            bool flag1 = false, flag2 = false;
            foreach (Instruction i in overrideMethod.Body.Instructions)
            {
                if (i.OpCode.Name.ToLower().Equals("stloc.0"))
                    flag1 = true;
                else if (!i.OpCode.Name.ToLower().Equals("ldloc.0"))
                    flag1 = false;

                if (flag1 && i.OpCode.Name.ToLower().Equals("ldloc.0"))
                    flag2 = true;

                if (!i.OpCode.Name.ToLower().Equals("newarr"))
                    unturnedMethod.Body.Instructions.Add(i);
                else
                    unturnedMethod.Body.Instructions.Add(Instruction.Create(OpCodes.Newarr, fieldType));

                    if (flag1 && flag2)
                {
                    for (int i2 = 0; i2 < combined.Length; i2++)
                    {
                        unturnedMethod.Body.Instructions.Add(Instruction.Create(OpCodes.Ldloc_0));
                        Instruction inst1 = Instruction.Create(OpCodes.Ldc_I4, (int)i2);
                        Instruction inst2 = Instruction.Create(OpCodes.Ldc_I4, (int)combined[i2]);
                        unturnedMethod.Body.Instructions.Add(inst1);
                        unturnedMethod.Body.Instructions.Add(inst2);
                        unturnedMethod.Body.Instructions.Add(Instruction.Create(OpCodes.Stelem_I1));
                    }
                }
            }
            Console.WriteLine("Fixed static hash");
        }

        public static byte[] combine(params byte[][] r)
        {
            byte[] array = new byte[r.Length * 20];

            for (int i = 0; i < r.Length; i++)
                r[i].CopyTo(array, (int)(i * 20));

            return SHA1(array);
        }

        public static byte[] SHA1(byte[] U)
        {
            SHA1CryptoServiceProvider sha = new SHA1CryptoServiceProvider();
            return sha.ComputeHash(U);
        }

        public static MethodDefinition GetMethod(TypeDefinition td, String method)
        {
            MethodDefinition md = null;

            foreach (MethodDefinition m in td.Methods)
                if (m.Name.ToLower().Equals(method.ToLower()))
                    md = m;

            return md;
        }

    }
}
