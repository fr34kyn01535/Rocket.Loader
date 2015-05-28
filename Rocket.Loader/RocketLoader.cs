using Mono.Cecil;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Rocket.RocketLoader
{
    public class RocketLoader
    {
        public static AssemblyDefinition UnityAssemblyDefinition, APIAssemblyDefinition, PatchAssemblyDefinition;
        public static Assembly PatchAssembly;

        private static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("RocketLoader Version " + Assembly.GetExecutingAssembly().GetName().Version.ToString());


                string apiAssemblyName = args[0]; //"Rocket.Unturned.dll";
                string unityAssemblyName = args[1]; //"Assembly-CSharp.dll";
                string patchAssemblyName = args[2]; //"Rocket.Loader.Unturned.dll";
                
                try
                {
                    PatchAssembly = Assembly.Load(File.ReadAllBytes(patchAssemblyName));
                    PatchAssemblyDefinition = AssemblyDefinition.ReadAssembly(patchAssemblyName);
                    UnityAssemblyDefinition = AssemblyDefinition.ReadAssembly(unityAssemblyName);
                    APIAssemblyDefinition = AssemblyDefinition.ReadAssembly(apiAssemblyName);
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
                    if (File.Exists(unityAssemblyName + ".original"))
                    {
                        File.Copy(unityAssemblyName + ".original", unityAssemblyName, true);
                        UnityAssemblyDefinition = AssemblyDefinition.ReadAssembly(unityAssemblyName);
                    }

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
                    Console.WriteLine("Backing up "+unityAssemblyName);
                    File.Copy(unityAssemblyName, unityAssemblyName + ".original", true);
                }

                foreach (var patch in PatchAssembly.GetTypes().Where(t => t.BaseType.FullName == "Rocket.RocketLoader.Patch").Select(t => Activator.CreateInstance(t) as Patch)) 
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

                UnityAssemblyDefinition.MainModule.Types.Add(new TypeDefinition("Rocket.RocketLoader", "Patched", new Mono.Cecil.TypeAttributes()));
                UnityAssemblyDefinition.Write(unityAssemblyName);
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
            return RocketLoader.UnityAssemblyDefinition.MainModule.GetType("Rocket.RocketLoader", "Patched") != null;
        }
    }
}