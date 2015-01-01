using Rocket.RocketAPI.Interfaces;
using SDG;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Rocket.RocketAPI.Managers
{
    public class PluginManager
    {
        internal List<RocketPlugin> plugins = new List<RocketPlugin>();
        
        internal PluginManager()
        {
            if (!Directory.Exists(Bootstrap.HomeFolder + "Plugins/")) Directory.CreateDirectory(Bootstrap.HomeFolder + "Plugins/");
            if (!Directory.Exists(Bootstrap.HomeFolder + "Plugins/Libraries/")) Directory.CreateDirectory(Bootstrap.HomeFolder + "Plugins/Libraries/");

            loadLibraries();
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(resolveHandler);
            loadPlugins();
        }

        private void loadLibraries()
        {
            FileInfo[] libraries = new DirectoryInfo(Bootstrap.HomeFolder + "Plugins/Libraries/").GetFiles("*.dll");
            foreach (FileInfo library in libraries)
            {
                try
                {
                    AssemblyName name = AssemblyName.GetAssemblyName(library.FullName);
                    assemblyNameToFileMapping.Add(name.FullName, library.FullName);
                    //Logger.Log(library.FullName + " is " + name.FullName);
                }
                catch { } 
            }
        }

        private Dictionary<string, string> assemblyNameToFileMapping = new Dictionary<string, string>();

        private Assembly resolveHandler(object sender, ResolveEventArgs args)
        {
            //Logger.Log("Trying to get " + args.Name);
            string file;
            if (assemblyNameToFileMapping.TryGetValue(args.Name, out file))
            {
                return Assembly.LoadFrom(file);
            }
            return null;
        }

        private void loadPlugins()
        {
            List<Type> pluginTypes = new List<Type>();
            try
            {    
                FileInfo[] pluginsLibraries = new DirectoryInfo(Bootstrap.HomeFolder + "Plugins/").GetFiles("*.dll");

                foreach (FileInfo library in pluginsLibraries)
                {
                    Assembly assembly = Assembly.LoadFile(library.FullName);
                    Type[] types;

                    try
                    {
                        types = assembly.GetTypes();
                    }
                    catch (ReflectionTypeLoadException e)
                    {
                        types = e.Types;
                    }

                    foreach (Type type in types.Where(t => t != null))
                    {
                        if (type.IsInterface || type.IsAbstract)
                        {
                            continue;
                        }
                        else
                        {
                            if (type.GetInterface(typeof(RocketPlugin).FullName) != null)
                            {
                                pluginTypes.Add(type);
                            }
                        }
                    }
                }
                foreach (Type pluginType in pluginTypes)
                {
                    try
                    {
                        RocketPlugin plugin = (RocketPlugin)Activator.CreateInstance(pluginType);
                        Logger.LogWarning("Loading: " + pluginType.Assembly.FullName);
                        plugin.Load();
                        plugins.Add(plugin);
                    }
                    catch (Exception e)
                    {
                        Logger.LogException(e);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        internal void Reload()
        {
            loadPlugins();
        }
    }
}
