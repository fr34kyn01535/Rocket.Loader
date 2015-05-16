using Mono.Cecil;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Rocket.RocketLoader
{
    public class RocketLoader
    {
        public static AssemblyDefinition UnturnedAssembly, LoaderAssembly, APIAssembly;//, UnityAssembly;

        private static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("RocketLoader Version " + Assembly.GetExecutingAssembly().GetName().Version.ToString());

                try
                {
                    //UnityAssembly = AssemblyDefinition.ReadAssembly("UnityEngine.dll");
                    UnturnedAssembly = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
                    APIAssembly = AssemblyDefinition.ReadAssembly("Rocket.Unturned.dll");
                    LoaderAssembly = AssemblyDefinition.ReadAssembly(System.Reflection.Assembly.GetExecutingAssembly().Location);
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error finding assemblies: " + e.ToString());
#if DEBUG
                    Console.ReadLine();
#endif
                    Environment.Exit(1);
                }

                if (isPatched())
                {
                    if (File.Exists("Assembly-CSharp.dll.bak"))
                    {
                        File.Copy("Assembly-CSharp.dll.bak", "Assembly-CSharp.dll", true);
                        UnturnedAssembly = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
                    }

                    //if (File.Exists("UnityEngine.dll.bak"))
                    //{
                    //    File.Copy("UnityEngine.dll.bak", "UnityEngine.dll", true);
                    //    UnityAssembly = AssemblyDefinition.ReadAssembly("UnityEngine.dll");
                    //}

                    if (isPatched())
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Unturned is already patched");
#if DEBUG
                        Console.ReadLine();
#endif
                        Environment.Exit(1);
                    }
                }
                else
                {

                    Console.WriteLine("Backing up Assembly-CSharp.dll");
                    File.Copy("Assembly-CSharp.dll", "Assembly-CSharp.dll.bak", true);

                    //Console.WriteLine("Backing up UnityEngine.dll");
                    //File.Copy("UnityEngine.dll", "UnityEngine.dll.bak", true);

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
                        Console.WriteLine("Error in " + patch.GetType().Name + ":" + ex.ToString());
#if DEBUG
                        Console.ReadLine();
#endif
                        Environment.Exit(1);
                    }
                }

                UnturnedAssembly.Write("Assembly-CSharp.dll");
                //UnityAssembly.Write("UnityEngine.dll");

                if (!(args.Count() == 1 && args[0] == "silent"))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Your game was successfully patched");
#if DEBUG
                    Console.ReadLine();
#endif
                    Environment.Exit(0);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: "+ ex.ToString());
#if DEBUG
                Console.ReadLine();
#endif
                Environment.Exit(1);
            }
        }


        private static bool isPatched()
        {
            MethodDefinition splash = RocketLoader.UnturnedAssembly.MainModule.GetType("SDG.CommandLine").Methods.AsEnumerable().Where(m => m.Name.ToLower() == "getcommands").FirstOrDefault();

            if (splash.Body.Instructions[0].ToString().Contains("Splash"))
            {
                return true;
            }
            return false;
        }
    }
}