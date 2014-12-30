using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using UnityEngine;
using SDG;
using System.Collections.Generic;
using Steamworks;
using System.Timers;


namespace Rocket.RocketAPI
{
    public class Core
    {
        public static String Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public static List<RocketPlugin> Plugins = new List<RocketPlugin>();
        public static List<RocketCommand> Commands = new List<RocketCommand>();
        public Core()
        {
            Logger.LogError("".PadLeft(80, '.'));
            Logger.LogError(@"                        ______           _        _ ");
            Logger.LogError(@"                        | ___ \         | |      | |");
            Logger.LogError(@"                        | |_/ /___   ___| | _____| |_");
            Logger.LogError(@"                        |    // _ \ / __| |/ / _ \ __|");
            Logger.LogError(@"                        | |\ \ (_) | (__|   <  __/ |_");
            Logger.LogError(@"                        \_| \_\___/ \___|_|\_\___|\__\ v" + Version + "\n");

            Logger.LogError("".PadLeft(80, '.'));

            Initialize(); 
        } 

        public void Initialize()
        {
            Permissions.Load();
            Plugins.Clear();
            Commands.Clear();

            Commands.Add(new CommandReload());
            Commands.Add(new CommandPlugins());
            Commands.Add(new CommandCheck());

            loadPlugins();
        }

        private void loadPlugins()
        {
            List<Type> pluginTypes = new List<Type>();
            try
            {
                FileInfo[] libraries = new DirectoryInfo("Servers/" + Bootstrap.InstanceName + "/Rocket/Plugins/").GetFiles("*.dll");

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
                        Plugins.Add(plugin);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Error in " + pluginType.Assembly.FullName + ": " + e.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

    }

}
