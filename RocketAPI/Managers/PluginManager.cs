using SDG;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Rocket.RocketAPI
{
    public class PluginManager
    {
        internal List<RocketPlugin> plugins = new List<RocketPlugin>();

        internal PluginManager()
        {
            loadPlugins();
        }

        private void loadPlugins()
        {
            if (!Directory.Exists(Bootstrap.HomeFolder + "Plugins/")) Directory.CreateDirectory(Bootstrap.HomeFolder + "Plugins/");

            List<Type> pluginTypes = new List<Type>();
            try
            {
                FileInfo[] libraries = new DirectoryInfo(Bootstrap.HomeFolder + "Plugins/").GetFiles("*.dll");

                foreach (FileInfo library in libraries)
                {
                    Assembly assembly = Assembly.LoadFile(library.FullName);
                    Type[] types = assembly.GetTypes();
                    foreach (Type type in types)
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
