using Rocket.API;
using Rocket.Core.Logging;
using Rocket.Core.Misc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Rocket.Core.Plugins
{
    public sealed class RocketPluginManager : MonoBehaviour
    {
        private static List<Assembly> pluginAssemblies;
        private static List<Component> plugins = new List<Component>();
        private Dictionary<string, string> additionalLibraries = new Dictionary<string, string>();

        private void Start()
        {
#if DEBUG
            Logger.Log("RocketPluginManager > Start");
#endif
            AppDomain.CurrentDomain.AssemblyResolve += delegate(object sender, ResolveEventArgs args)
            {
                string file;
                if (additionalLibraries.TryGetValue(args.Name, out file))
                {
                    return Assembly.Load(File.ReadAllBytes(file));
                }
                else
                {
                    Logger.LogError("Could not find dependency: " + args.Name);
                }
                return null;
            };

            additionalLibraries = loadAdditionalAssemblies(RocketBootstrap.Implementation.LibrariesFolder);
            pluginAssemblies = loadPluginAssemblies(RocketBootstrap.Implementation.PluginsFolder);
            List<Type> pluginImplemenations = RocketHelper.GetTypesFromInterface(pluginAssemblies, "IRocketPlugin");
#if DEBUG
          //  Logger.Log("Libraries: " + String.Join(", ", additionalLibraries.Select(a => a.Key).ToArray()));
            Logger.Log("Plugins: " + String.Join(", ", pluginAssemblies.Select(a => a.GetName().Name).ToArray()));
            Logger.Log("Plugin classes: " + String.Join(", ", pluginImplemenations.Select(a => a.Name).ToArray()));
#endif
            foreach (Type plugin in pluginImplemenations)
            {
                plugins.Add(RocketBootstrap.Instance.gameObject.AddComponent(plugin));
            }
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Done".PadRight(80, '.'));
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static IRocketPlugin GetPlugin(string name) {
            Assembly assembly = pluginAssemblies.Where(a => a.GetName().Name.ToLower().Contains(name.ToLower())).FirstOrDefault();
            if (assembly == null) return null;

            Type plugin = RocketHelper.GetTypesFromParentClass(assembly, typeof(IRocketPlugin)).FirstOrDefault();

            return GetPlugin(plugin);
        }

        public static IRocketPlugin GetPlugin(Type plugin)
        {
            Component c = RocketBootstrap.Instance.gameObject.GetComponent(plugin);
            return (c is IRocketPlugin) ? (IRocketPlugin)c : null;
        }

        public static string[] GetPluginNames() {
            return pluginAssemblies.Select(a => a.GetName().Name).ToArray();
        }

        public static List<IRocketPlugin> GetPlugins()
        {
            return plugins.Select(p => (IRocketPlugin)p).ToList();
        }

        private Dictionary<string, string> loadAdditionalAssemblies(string directory)
        {
            Dictionary<string, string> l = new Dictionary<string, string>(); 
            IEnumerable<FileInfo> libraries = new DirectoryInfo(directory).GetFiles("*.dll", SearchOption.AllDirectories).Where(f => f.Extension == ".dll");
            foreach (FileInfo library in libraries)
            {
                try
                {
                    AssemblyName name = AssemblyName.GetAssemblyName(library.FullName);
                    l.Add(name.FullName, library.FullName);
                }
                catch { }
            }
            return l;
        }
     
        private static List<Assembly> loadPluginAssemblies(string directory)
        {
            List<Assembly> assemblies = new List<Assembly>();
            try
            {
                IEnumerable<FileInfo> pluginsLibraries = new DirectoryInfo(directory).GetFiles("*.dll", SearchOption.TopDirectoryOnly).Where(f => f.Extension == ".dll");

                foreach (FileInfo library in pluginsLibraries)
                {
                    Assembly assembly = Assembly.Load(File.ReadAllBytes(library.FullName));
                   // Logger.Log(assembly.GetName().Name + " Version: " + assembly.GetName().Version);
                    assemblies.Add(assembly);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return assemblies;
        }
    }
}