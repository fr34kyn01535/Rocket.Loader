using Rocket.RocketAPI;
using Rocket.RocketAPI.Components;
using SDG;
using Steamworks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Rocket
{
    public class RocketPluginManager : RocketManagerComponent
    {
        public List<Assembly> Assemblies;

        private new void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(transform.gameObject);
            #region Handling additional assemblies
            AppDomain.CurrentDomain.AssemblyResolve += delegate(object sender, ResolveEventArgs args)
            {
                string file;
                if (additionalLibraries.TryGetValue(args.Name, out file))
                {
                    return Assembly.Load(File.ReadAllBytes(file));
                }
                else {
                    Logger.LogError("Could not find additional Library " + file);
                }
                return null;
            };
            loadLibraries();
            #endregion

            Logger.LogError("\nLoading Extensions".PadRight(80, '.') + "\n");
            Assemblies = loadAssemblies();

            List<Type> rocketManagerComponents = getTypes(Assemblies, typeof(RocketManagerComponent));
            rocketManagerComponents.Add(typeof(RocketManager));
            rocketManagerComponents.Add(typeof(RocketPermissionManager));

            foreach (Type component in rocketManagerComponents)
            {
                RocketLauncher.Instance.gameObject.AddComponent(component);
            }

            Logger.LogError("\nLoading commands".PadRight(80, '.') + "\n");
            List<Type> commands = getTypes(Assemblies, typeof(Command));

            foreach (Type command in commands)
            {
                registerCommand((Command)Activator.CreateInstance(command));
            }
            SDG.Steam.OnServerConnected += onPlayerConnected;
        }

        internal static void registerCommand(Command command)
        {
            List<Command> commandList = new List<Command>();
            bool msg = false;
            foreach (Command ccommand in Commander.Commands)
            {
                if (ccommand.commandName.ToLower().Equals(command.commandName.ToLower()))
                {
                    if (ccommand.GetType().Assembly.GetName().Name == "Assembly-CSharp")
                    {
                        Logger.LogWarning(command.GetType().Assembly.GetName().Name + "." + command.commandName + " overwrites built in command " + ccommand.commandName);
                        msg = true;
                    }
                    else
                    {
                        Logger.LogError("Can not register command " + command.GetType().Assembly.GetName().Name + "." + command.commandName + " because its already registered by " + ccommand.GetType().Assembly.GetName().Name + "." + ccommand.commandName);
                        return;
                    }
                }
                else
                {
                    commandList.Add(ccommand);
                }
            }

            if (!msg) Logger.Log(command.GetType().Assembly.GetName().Name + "." + command.commandName);
            commandList.Add(command);
            Commander.Commands = commandList.ToArray();
        }

        void onPlayerConnected(CSteamID id)
        {
            Player player = PlayerTool.getPlayer(id);
            List<Type> rocketPlayerComponents = getTypes(Assemblies, typeof(RocketPlayerComponent));
            rocketPlayerComponents.Add(typeof(Events));

            GameObject gameobject = player.transform.gameObject;
            foreach (Type component in rocketPlayerComponents)
            {
                gameobject.AddComponent(component);
            }
        }

        #region Handling additional assemblies
        private Dictionary<string, string> additionalLibraries = new Dictionary<string, string>();
        private void loadLibraries()
        {
            IEnumerable<FileInfo> libraries = new DirectoryInfo(RocketSettings.HomeFolder + "Plugins/Libraries/").GetFiles("*.dll", SearchOption.AllDirectories).Where(f => f.Extension == ".dll");
            foreach (FileInfo library in libraries)
            {
                try
                {
                    AssemblyName name = AssemblyName.GetAssemblyName(library.FullName);
                    additionalLibraries.Add(name.FullName, library.FullName);
                }
                catch { } 
            }
        }
        #endregion

        private static List<Assembly> loadAssemblies()
        {
            List<Assembly> assemblies = new List<Assembly>();
            try
            {
                IEnumerable<FileInfo> pluginsLibraries = new DirectoryInfo(RocketSettings.HomeFolder + "Plugins/").GetFiles("*.dll", SearchOption.TopDirectoryOnly).Where(f => f.Extension == ".dll");

                foreach (FileInfo library in pluginsLibraries)
                {
                    Assembly assembly = Assembly.Load(File.ReadAllBytes(library.FullName));
                    Logger.Log(assembly.GetName().Name + " Version: " + assembly.GetName().Version);
                    assemblies.Add(assembly);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return assemblies;
        }

        internal static List<Type> getTypes(List<Assembly> assemblies)
        {
            List<Type> allTypes = new List<Type>();
            foreach (Assembly assembly in assemblies) {
                Type[] types;
                try
                {
                    types = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException e)
                {
                    types = e.Types;
                }
                allTypes.AddRange(types);
            }
            return allTypes;
        }
        internal static List<Type> getTypes(List<Assembly> assemblies,Type parentClass)
        {
            List<Type> allTypes = new List<Type>();
            foreach (Assembly assembly in assemblies)
            {
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
                    if (type.IsSubclassOf(parentClass))
                    {
                        allTypes.Add(type);
                    }
                }
            }
            return allTypes;
        }
    }
}
