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
    class RocketLoader
    {
        static String version = "1.0";

        static AssemblyDefinition unturnedAssembly, unturnedFirstpassAssembly, patcherAssembly, loaderAssembly;

        static void Main(string[] args)
        {
            Console.WriteLine("RocketLoader Version " + version);

            try
            {
                unturnedAssembly = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
                patcherAssembly = AssemblyDefinition.ReadAssembly(System.Reflection.Assembly.GetExecutingAssembly().Location);
                loaderAssembly = AssemblyDefinition.ReadAssembly("RocketAPI.dll");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadKey();
                Environment.Exit(1);
            }

            attachBootstrap();

            fixHash();

            patchChatMan();
            patchCommander();
            patchCommand();

            //patchEverything(); 

            removePlayButton(); 

            Console.WriteLine("Writing to file...");
            unturnedAssembly.Write("Assembly-CSharp.dll");
        }

        private static void patchCommand()
        {
            Console.WriteLine("Patching command");
            TypeDefinition td = unturnedAssembly.MainModule.GetType("SDG.Command");

            FieldDefinition[] stringFields = td.Fields.Where(f => f.FieldType.FullName.ToLower() == "system.string").ToArray();

                stringFields[0].Name = "commandName";
                stringFields[0].IsPublic = true;
                stringFields[0].IsPrivate = false;
                Console.WriteLine("Patching CommandName");
            
                stringFields[1].Name = "commandInfo";
                stringFields[1].IsPublic = true;
                stringFields[1].IsPrivate = false;
                Console.WriteLine("Patching CommandName");

                stringFields[2].Name = "commandHelp";
                stringFields[2].IsPublic = true;
                stringFields[2].IsPrivate = false;
                Console.WriteLine("Patching CommandName");

        }

        private static void patchCommander()
        {
            Console.WriteLine("Patching commands");
            TypeDefinition td = unturnedAssembly.MainModule.GetType("SDG.Commander");

            foreach (FieldDefinition f in td.Fields)
            {
                if (f.FieldType.FullName.ToLower() == "sdg.command[]")
                {
                    f.IsPublic = true;
                    f.IsPrivate = false;
                    f.Name = "commandList";
                    Console.WriteLine("Commandlist unlocked");
                }
            }
        }

        private static void removePlayButton()
        {
            Console.WriteLine("Removing Play Button");
            TypeDefinition td = unturnedAssembly.MainModule.GetType("SDG.MenuPlayUI");
            MethodDefinition ctor = getMethod(td, ".ctor");
            ctor.Body.Instructions.Clear();
        }

        private static void patchChatMan()
        {
            TypeDefinition td = unturnedAssembly.MainModule.GetType("SDG.ChatManager");

            foreach (FieldDefinition f in td.Fields)
            {
                if (f.FieldType.FullName.ToLower() == "sdg.chat[]")
                {
                    f.IsPublic = true;
                    f.IsPrivate = false;
                    f.Name = "chatLog";
                    Console.WriteLine("Chatlog unlocked");
                }
            }

            Console.WriteLine("Done patching chatmanager!");
        }

        private static void patchEverything()
        {
            foreach (TypeDefinition td in unturnedAssembly.MainModule.GetTypes()) {
                if (td.Name == "Level") continue;
                try
                {
                    foreach (FieldDefinition f in td.Fields)
                    {
                        if(!f.IsPublic && f.IsPrivate)
                        f.IsPublic = true;
                        f.IsPrivate = false;
                    }

                    foreach (MethodDefinition m in td.Methods)
                    {
                        m.IsPublic = true;
                        m.IsPrivate = false;
                    }
            
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed unlocking: "+td.Name);
                }
            }
        }

        private static void attachBootstrap()
        {
            TypeDefinition loaderType = loaderAssembly.MainModule.GetType("Rocket.RocketAPI.Bootstrap");
            MethodDefinition initBootstrap = getMethod(loaderType, "LaunchRocket");

            TypeDefinition unturnedType = unturnedAssembly.MainModule.GetType("SDG.Managers");
            MethodDefinition awake = getMethod(unturnedType, "Awake");

            awake.Body.GetILProcessor().InsertBefore(awake.Body.Instructions[0], Instruction.Create(OpCodes.Call, unturnedAssembly.MainModule.Import(initBootstrap)));
            Console.WriteLine("Attached Bootstrap");
        }

        private static void fixHash()
        {
            byte[] unturned, unturned_firstpass, other, other_firstpass;
            using (FileStream csharpFilestream = new FileStream("Assembly-CSharp.dll", FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                unturned = new byte[csharpFilestream.Length];
            }

            using (FileStream csharpFilestream = new FileStream("Assembly-CSharp-firstpass.dll", FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                unturned_firstpass = new byte[csharpFilestream.Length];
            }

            using (FileStream csharpFilestream = new FileStream("Other-Assembly-CSharp.dll", FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                other = new byte[csharpFilestream.Length];
            }

            using (FileStream csharpFilestream = new FileStream("Other-Assembly-CSharp-firstpass.dll", FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                other_firstpass = new byte[csharpFilestream.Length];
            }

            byte[] combined = combine(new byte[][] { SHA1(unturned), SHA1(unturned_firstpass), SHA1(other), SHA1(other_firstpass) });


            MethodDefinition unturnedMethod = getMethod(unturnedAssembly.MainModule.GetType("SDG.ReadWrite"), "getAssemblyHash");
            MethodDefinition overrideMethod = getMethod(patcherAssembly.MainModule.GetType("Rocket.RocketLoader"), "getAssemblyHash");

            TypeReference byteType = AssemblyDefinition.ReadAssembly("mscorlib.dll").MainModule.GetType("System.Byte");

            Console.WriteLine("Overriding hash check");

            unturnedMethod.Body.Instructions.Clear();

            Console.WriteLine("Cleared hash func, injecting new code");

            TypeReference fieldType = unturnedAssembly.MainModule.Import(byteType);

            foreach (Instruction i in overrideMethod.Body.Instructions)
            {
                if (i.OpCode.Name.ToLower().Equals("newarr"))
                    unturnedMethod.Body.Instructions.Add(Instruction.Create(OpCodes.Newarr, fieldType));
                else
                    unturnedMethod.Body.Instructions.Add(i);

                if (i.OpCode.Name.ToLower().Equals("stloc.0") && i.OpCode.Name.ToLower().Equals("ldloc.0"))
                {
                    for (int i2 = 0; i2 < combined.Length; i2++)
                    {
                        unturnedMethod.Body.Instructions.Add(Instruction.Create(OpCodes.Ldloc_0));
                        Instruction inst1 = Instruction.Create(OpCodes.Ldc_I4, (int)i2);
                        Instruction inst2 = Instruction.Create(OpCodes.Ldc_I4, (int)combined[i2]);
                        unturnedMethod.Body.Instructions.Add(inst1);
                        unturnedMethod.Body.Instructions.Add(inst2);
                        unturnedMethod.Body.Instructions.Add(Instruction.Create(OpCodes.Stelem_I1));
                        Console.WriteLine("Fixed byte: " + i2 + ", " + combined[i2]);
                    }
                }
            }
            Console.WriteLine("Fixed static hash");
        }

        /*This method will be injected into Unturned, do not touch!*/
        public static System.Byte[] getAssemblyHash()
        {
            byte[] b = new System.Byte[20];
            return b;
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

        public static MethodDefinition getMethod(TypeDefinition td, String method)
        {
            MethodDefinition md = null;

            foreach (MethodDefinition m in td.Methods)
                if (m.Name.ToLower().Equals(method.ToLower()))
                    md = m;

            return md;
        }

    }
}
