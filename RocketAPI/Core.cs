using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using UnityEngine;
using SDG;
using System.Collections.Generic;


namespace Rocket.RocketAPI
{
    public class Core
    {
        public static String Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public static List<RocketPlugin> Plugins = new List<RocketPlugin>();
        
        public void Initialize()
        {
            SDG.Commander.init();
            Logger.LogError("".PadLeft(80, '.'));
            Logger.LogError(@"                        ______           _        _ ");
            Logger.LogError(@"                        | ___ \         | |      | |");
            Logger.LogError(@"                        | |_/ /___   ___| | _____| |_");
            Logger.LogError(@"                        |    // _ \ / __| |/ / _ \ __|");
            Logger.LogError(@"                        | |\ \ (_) | (__|   <  __/ |_");
            Logger.LogError(@"                        \_| \_\___/ \___|_|\_\___|\__\ v" + Version + "\n");

            Logger.LogError("".PadLeft(80, '.'));

            if (!Directory.Exists("Unturned_Data/Managed/Plugins/")) Directory.CreateDirectory("Unturned_Data/Managed/Plugins/");
            
            Commands.RegisterCommand(new CommandReload());
            Commands.RegisterCommand(new CommandPlugins());
            /*
            Commands.RegisterCommand(new CommandReloot());*/

            Permissions.LoadConfiguration();
            LoadPlugins();
        }

        public void LoadPlugins(){
            Plugins.Clear();
            List<Type> pluginTypes = loadPlugins();
            executePlugins(pluginTypes);
            Permissions.LoadConfiguration();
        }

        private void executePlugins(List<Type> pluginTypes)
        {
            foreach (Type pluginType in pluginTypes)
            {
                Logger.LogWarning("Loading: " + pluginType.Assembly.FullName);
                try
                {
                    //Bootstrap.kGameObject.AddComponent(pluginType);
                    RocketPlugin plugin = (RocketPlugin)Activator.CreateInstance(pluginType);
                    plugin.Load();
                    Plugins.Add(plugin);
                }
                catch (Exception e)
                {
                    Debug.LogError("Error in " + pluginType.Assembly.FullName + ": " + e.ToString());
                }
            }
        }


        private List<Type> loadPlugins()
        {
            List<Type> plugins = new List<Type>();
            try
            {
                FileInfo[] libraries = new DirectoryInfo("Unturned_Data/Managed/Plugins/").GetFiles("*.dll");

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
                                plugins.Add(type);
                            }
                        } 
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return plugins;
        }

    }

}
