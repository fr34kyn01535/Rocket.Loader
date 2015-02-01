using Rocket.RocketAPI.Components;
using SDG;
using Steamworks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Rocket.RocketAPI
{
    public class RocketPluginManager : RocketManagerComponent
    {
        public List<Assembly> Assemblies;

        private void Start()
        {
#if !DEBUG
            RocketLoadingAnimation.Stop();
            Console.Clear();
#else
            Logger.Log("Start RocketPluginManager");
#endif

            #region Handling additional assemblies

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
            loadLibraries();

            #endregion Handling additional assemblies

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("The rocket has launched | v" + Assembly.GetExecutingAssembly().GetName().Version.ToString() + "\n");

            if (RocketSettings.EnableRcon)
            {
                Console.WriteLine("Loading RocketRcon".PadRight(80, '.'));
                RocketRconManager.Listen();
                Console.WriteLine();
            }

            Console.WriteLine("Loading Extensions".PadRight(80, '.'));
            Assemblies = loadAssemblies();

            List<Type> rocketManagerComponents = getTypes(Assemblies, typeof(RocketManagerComponent));
            /*The API rocketmanagers are loaded in the RocketLauncher, they dont have to be included here*/

            foreach (Type component in rocketManagerComponents)
            {
                RocketLauncher.Instance.gameObject.AddComponent(component);
            }
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nLoading commands".PadRight(80, '.') + "\n");
            /*But now i could also use the API commands & players loaded */
            Assemblies.Add(Assembly.GetExecutingAssembly());
            /*so i add the rocketapi to Assemblies*/
            List<Type> commands = getTypes(Assemblies, typeof(Command));
            foreach (Type command in commands)
            {
                registerCommand((Command)Activator.CreateInstance(command));
            }
            SDG.Steam.OnServerConnected += onPlayerConnected;

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nLaunching Unturned".PadRight(80, '.'));
            Logger.LogWarning("\nThe error concerning a corrupted file resourcs.assets can be");
            Logger.LogWarning("ignored while we work on a bugfix".PadRight(79, '.') + "\n");

            SteamGameServer.SetBotPlayerCount(0);
            string v = "Unturned Rocket " + Assembly.GetExecutingAssembly().GetName().Version.ToString();
            SteamGameServer.SetModDir(v);
            SteamGameServer.SetGameTags(v);
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

        private void onPlayerConnected(CSteamID id)
        {
            SDG.Player player = PlayerTool.getPlayer(id);
            List<Type> rocketPlayerComponents = getTypes(Assemblies, typeof(RocketPlayerComponent));
#if DEBUG
            Logger.Log("Adding PlayerComponents");
#endif
            foreach (Type component in rocketPlayerComponents)
            {
#if DEBUG
                Logger.Log(component.Name);
#endif
                player.gameObject.AddComponent(component);
            }
        }

        #region Handling additional assemblies

        private Dictionary<string, string> additionalLibraries = new Dictionary<string, string>();

        private void loadLibraries()
        {
            IEnumerable<FileInfo> libraries = new DirectoryInfo(RocketSettings.HomeFolder + "Libraries/").GetFiles("*.dll", SearchOption.AllDirectories).Where(f => f.Extension == ".dll");
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

        #endregion Handling additional assemblies

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
                allTypes.AddRange(types);
            }
            return allTypes;
        }

        internal static List<Type> getTypes(List<Assembly> assemblies, Type parentClass)
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