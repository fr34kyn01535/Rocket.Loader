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

        static AssemblyDefinition unturnedAssembly, patcherAssembly, loaderAssembly;

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

            patchPlayer();
            patchChatMan();
            patchSteamCalls();
            patchCommander();
            patchCommand();

            //patchEverything(); 

            removePlayButton(); 

            Console.WriteLine("Writing to file...");
            unturnedAssembly.Write("Assembly-CSharp.dll");
        }

        private static void patchPlayer()
        {

            TypeDefinition td = unturnedAssembly.MainModule.GetType("SDG.Player");

            foreach (FieldDefinition f in td.Fields)
            {
                if (f.FieldType.FullName.ToLower() == "sdg.playerinput")
                {
                    f.IsPublic = true;
                    f.IsPrivate = false;
                    f.Name = "playerInput";
                    Console.WriteLine("PlayerInput unlocked");
                }

                if (f.FieldType.FullName.ToLower() == "sdg.playerstance")
                {
                    f.IsPublic = true;
                    f.IsPrivate = false;
                    f.Name = "playerStance";
                    Console.WriteLine("PlayerStance unlocked");
                }

                if (f.FieldType.FullName.ToLower() == "sdg.playerequipment")
                {
                    f.IsPublic = true;
                    f.IsPrivate = false;
                    f.Name = "playerEquipment";
                    Console.WriteLine("PlayerEquipment unlocked");
                }

                if (f.FieldType.FullName.ToLower() == "sdg.playeranimator")
                {
                    f.IsPublic = true;
                    f.IsPrivate = false;
                    f.Name = "playerAnimator";
                    Console.WriteLine("PlayerAnimator unlocked");
                }

                if (f.FieldType.FullName.ToLower() == "sdg.playerhitbox")
                {
                    f.IsPublic = true;
                    f.IsPrivate = false;
                    f.Name = "playerHitbox";
                    Console.WriteLine("PlayerHitbox unlocked");
                }

                if (f.FieldType.FullName.ToLower() == "sdg.playermovement")
                {
                    f.IsPublic = true;
                    f.IsPrivate = false;
                    f.Name = "playerMovement";
                    Console.WriteLine("PlayerMovement unlocked");
                }

                if (f.FieldType.FullName.ToLower() == "sdg.playerlook")
                {
                    f.IsPublic = true;
                    f.IsPrivate = false;
                    f.Name = "playerLook";
                    Console.WriteLine("PlayerLook unlocked");
                }

                if (f.FieldType.FullName.ToLower() == "sdg.playerclothing")
                {
                    f.IsPublic = true;
                    f.IsPrivate = false;
                    f.Name = "playerClothing";
                    Console.WriteLine("PlayerClothing unlocked");
                }

                if (f.FieldType.FullName.ToLower() == "sdg.playerinventory")
                {
                    f.IsPublic = true;
                    f.IsPrivate = false;
                    f.Name = "playerInventory";
                    Console.WriteLine("PlayerInventory unlocked");
                }

                if (f.FieldType.FullName.ToLower() == "sdg.playerlife")
                {
                    f.IsPublic = true;
                    f.IsPrivate = false;
                    f.Name = "playerLife";
                    Console.WriteLine("PlayerLife unlocked");
                }

            }

            Console.WriteLine("Done patching player!");

        }

        private static void patchSteamCalls()
        {

            TypeDefinition td = unturnedAssembly.MainModule.GetType("SDG.Steam");

            foreach (FieldDefinition f in td.Fields)
            {
                if (f.FieldType.FullName.ToLower() == "sdg.steam/clientconnected")
                {
                    f.IsPublic = true;
                    f.IsPrivate = false;
                    f.Name = "clientConnected";
                    Console.WriteLine("ClientConnected unlocked");
                }

                if (f.FieldType.FullName.ToLower() == "sdg.steam/clientdisconnected")
                {
                    f.IsPublic = true;
                    f.IsPrivate = false;
                    f.Name = "clientDisconnected";
                    Console.WriteLine("ClientDisconnected unlocked");
                }

                if (f.FieldType.FullName.ToLower() == "sdg.steam/serverhosted")
                {
                    f.IsPublic = true;
                    f.IsPrivate = false;
                    f.Name = "serverHosted";
                    Console.WriteLine("ServerConnected unlocked");
                }

                if (f.FieldType.FullName.ToLower() == "sdg.steam/servershutdown")
                {
                    f.IsPublic = true;
                    f.IsPrivate = false;
                    f.Name = "serverShutdown";
                    Console.WriteLine("ServerShutdown unlocked");
                }

                if (f.FieldType.FullName.ToLower() == "sdg.steam/serverconnected")
                {
                    f.IsPublic = true;
                    f.IsPrivate = false;
                    f.Name = "serverConnected";
                    Console.WriteLine("ServerConnected unlocked");
                }

                if (f.FieldType.FullName.ToLower() == "sdg.steam/serverdisconnected")
                {
                    f.IsPublic = true;
                    f.IsPrivate = false;
                    f.Name = "serverDisconnected";
                    Console.WriteLine("ServerDisconnected unlocked");
                }
            }

            Console.WriteLine("Done patching steamcalls!");

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

            if (awake.Body.Instructions[0].ToString().Contains("LaunchRocket")) {
                Console.WriteLine("Already patched!");
                Environment.Exit(0);
            }

            awake.Body.GetILProcessor().InsertBefore(awake.Body.Instructions[0], Instruction.Create(OpCodes.Call, unturnedAssembly.MainModule.Import(initBootstrap)));
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


            MethodDefinition unturnedMethod = getMethod(unturnedAssembly.MainModule.GetType("SDG.ReadWrite"), "getAssemblyHash");
            MethodDefinition overrideMethod = getMethod(patcherAssembly.MainModule.GetType("Rocket.RocketLoader"), "getAssemblyHash");

            TypeReference byteType = AssemblyDefinition.ReadAssembly("mscorlib.dll").MainModule.GetType("System.Byte");

            Console.WriteLine("Overriding hash check");

            unturnedMethod.Body.Instructions.Clear();

            Console.WriteLine("Cleared hash func, injecting new code");

            TypeReference fieldType = unturnedAssembly.MainModule.Import(byteType);
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
