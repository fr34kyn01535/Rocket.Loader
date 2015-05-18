using Mono.Cecil;
using Mono.Cecil.Cil;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace Rocket.RocketLoader.Patches
{
    public class ReadWrite : IPatch
    {
        private PatchHelper h = new PatchHelper("SDG.ReadWrite");

        public static byte[] combine(params byte[][] r)
        {
            byte[] array = new byte[r.Length * 20];

            for (int i = 0; i < r.Length; i++)
                r[i].CopyTo(array, (int)(i * 20));

            return SHA1(array);
        }

        static SHA1CryptoServiceProvider sha = new SHA1CryptoServiceProvider();

        public static byte[] SHA1(byte[] U)
        {
            return sha.ComputeHash(U);
        }

        public void Apply()
        {
            byte[] unturned, unturned_firstpass, other, other_firstpass, other2, other2_firstpass;
            using (FileStream filestream = new FileStream("Assembly-CSharp.dll", FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                unturned = new byte[filestream.Length];
            }

            using (FileStream filestream = new FileStream("Other-Assembly-CSharp.dll", FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                other = new byte[filestream.Length];
            }

            using (FileStream filestream = new FileStream("Other2-Assembly-CSharp.dll", FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                other2 = new byte[filestream.Length];
            }


            using (FileStream filestream = new FileStream("Assembly-CSharp-firstpass.dll", FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                unturned_firstpass = new byte[filestream.Length];
            }

            using (FileStream filestream = new FileStream("Other-Assembly-CSharp-firstpass.dll", FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                other_firstpass = new byte[filestream.Length];
            }

            using (FileStream filestream = new FileStream("Other2-Assembly-CSharp-firstpass.dll", FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                other2_firstpass = new byte[filestream.Length];
            }

            byte[] combined = combine(new byte[][] { SHA1(unturned), SHA1(other), SHA1(other2), SHA1(unturned_firstpass), SHA1(other_firstpass), SHA1(other2_firstpass) });

            MethodDefinition getAssemblyHash = h.GetMethod("getAssemblyHash");
            getAssemblyHash.Body.Instructions.Clear();

            MethodDefinition newgetAssemblyHash = RocketLoader.LoaderAssembly.MainModule.GetType("Rocket.RocketLoader.Patches.ReadWrite").Methods.AsEnumerable().Where(m => m.Name == "getAssemblyHash").FirstOrDefault();

            TypeReference byteType = AssemblyDefinition.ReadAssembly("mscorlib.dll").MainModule.GetType("System.Byte");

            TypeReference fieldType = RocketLoader.UnturnedAssembly.MainModule.Import(byteType);
            bool flag1 = false, flag2 = false;

            //TODO fix this shit

            foreach (Instruction i in newgetAssemblyHash.Body.Instructions)
            {
                if (i.OpCode.Name.ToLower().Equals("stloc.0"))
                    flag1 = true;
                else if (!i.OpCode.Name.ToLower().Equals("ldloc.0"))
                    flag1 = false;

                if (flag1 && i.OpCode.Name.ToLower().Equals("ldloc.0"))
                    flag2 = true;

                if (!i.OpCode.Name.ToLower().Equals("newarr"))
                    getAssemblyHash.Body.Instructions.Add(i);
                else
                    getAssemblyHash.Body.Instructions.Add(Instruction.Create(OpCodes.Newarr, fieldType));

                if (flag1 && flag2)
                {
                    for (int i2 = 0; i2 < combined.Length; i2++)
                    {
                        getAssemblyHash.Body.Instructions.Add(Instruction.Create(OpCodes.Ldloc_0));
                        Instruction inst1 = Instruction.Create(OpCodes.Ldc_I4, (int)i2);
                        Instruction inst2 = Instruction.Create(OpCodes.Ldc_I4, (int)combined[i2]);
                        getAssemblyHash.Body.Instructions.Add(inst1);
                        getAssemblyHash.Body.Instructions.Add(inst2);
                        getAssemblyHash.Body.Instructions.Add(Instruction.Create(OpCodes.Stelem_I1));
                    }
                }
            }
        }

        private static System.Byte[] getAssemblyHash()
        {
            byte[] b = new System.Byte[20];
            return b;
        }
    }
}