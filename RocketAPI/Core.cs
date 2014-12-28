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

        public Core()
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
            if (!Directory.Exists("Unturned_Data/Managed/Plugins/" + Bootstrap.InstanceName)) Directory.CreateDirectory("Unturned_Data/Managed/Plugins/" + Bootstrap.InstanceName);
            
            FileInfo[] libraries = new DirectoryInfo("Unturned_Data/Managed/Plugins/").GetFiles("*.dll");

            foreach (FileInfo library in libraries)
            {
                if (!File.Exists("Unturned_Data/Managed/Plugins/" + Bootstrap.InstanceName+"/"+library.Name))
                {
                    File.Copy(library.FullName, "Unturned_Data/Managed/Plugins/" + Bootstrap.InstanceName + "/" + library.Name);
                }
                File.Delete(library.FullName);
            }

            LoadPlugins();
        }

        public void LoadPlugins()
        {
            Permissions.Load();
            Commander.init();
            Plugins.Clear();

            Commands.RegisterCommand(new CommandReload());
            Commands.RegisterCommand(new CommandPlugins());
            /*Commands.RegisterCommand(new CommandReloot());*/

            loadPlugins();
        }

        private void loadPlugins()
        {
            List<Type> pluginTypes = new List<Type>();
            try
            {
                FileInfo[] libraries = new DirectoryInfo("Unturned_Data/Managed/Plugins/" + Bootstrap.InstanceName).GetFiles("*.dll");

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
