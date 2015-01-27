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

namespace Rocket.RocketLoader
{
    public class RocketLoader
    {
        public static AssemblyDefinition UnturnedAssembly, LoaderAssembly, APIAssembly;

        static void Main(string[] args)
        {
            Console.WriteLine("RocketLoader Version " + Assembly.GetExecutingAssembly().GetName().Version.ToString());
           
            try
            {
                UnturnedAssembly = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
                APIAssembly = AssemblyDefinition.ReadAssembly("RocketAPI.dll");
                LoaderAssembly = AssemblyDefinition.ReadAssembly(System.Reflection.Assembly.GetExecutingAssembly().Location);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e);
                Console.WriteLine("Press any key to quit");
                Console.ReadKey();
                Environment.Exit(1);
            }

            if (isPatched())
            {
                if (File.Exists("Assembly-CSharp.dll.bak"))
                {
                    File.Copy("Assembly-CSharp.dll.bak", "Assembly-CSharp.dll", true);
                    UnturnedAssembly = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
                }

                if (isPatched())
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Unturned is already patched");
                    Console.WriteLine("Press any key to quit");
                    Console.ReadKey();
                    Environment.Exit(1);
                }
            }
            else
            {
                Console.WriteLine("Backing up Assembly-CSharp.dll");
                File.Copy("Assembly-CSharp.dll", "Assembly-CSharp.dll.bak", true);
            }

            var patches = from t in Assembly.GetExecutingAssembly().GetTypes()
                          where t.GetInterfaces().Contains(typeof(Patch)) && t.GetConstructor(Type.EmptyTypes) != null
                          select Activator.CreateInstance(t) as Patch;

            foreach (var patch in patches)
            {
                try
                {
                    patch.Apply();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error in "+patch.GetType().Name+":"+ex.ToString());
                    Console.ReadLine();
                }
            }

            UnturnedAssembly.Write("Assembly-CSharp.dll");


            if (!(args.Count() == 1 && args[0] == "silent"))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Your game was successfully patched");
                Console.WriteLine("Press any key to quit");
                Console.ReadKey();
            }
        }

        private static bool isPatched()
        {
            MethodDefinition awake = RocketLoader.UnturnedAssembly.MainModule.GetType("SDG.Managers").Methods.AsEnumerable().Where(m => m.Name.ToLower() == "awake").FirstOrDefault();

            if (awake.Body.Instructions[0].ToString().Contains("Launch"))
            {
                return true;
            }
            return false;
        }
    }
}
