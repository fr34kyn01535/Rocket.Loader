using Rocket.Commands;
using Rocket.Components;
using Rocket.Logging;
using Rocket.Rcon;
using SDG;
using Steamworks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Rocket.RocketAPI
{
    public sealed class RocketPluginManager : RocketManagerComponent
    {
        public List<Assembly> Assemblies;

        private void Start()
        {
#if !DEBUG
            //RocketLoadingAnimation.Stop();
            //Console.Clear();
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

            Console.WriteLine();

            if (RocketSettings.EnableRcon)
            {
                Console.WriteLine("Loading RocketRcon".PadRight(80, '.'));
                RocketRconServer.Listen();
                Console.WriteLine();
            }

            string[] whitelist = RocketPermissionManager.GetWhitelistedGroups();
            if (whitelist != null && whitelist.Count() != 0)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Whitelist following groups".PadRight(80, '.'));
                foreach (string s in whitelist) {
                    Console.WriteLine(s);
                }
                Console.WriteLine();
            }

            Console.WriteLine("Loading Plugins".PadRight(80, '.'));
            Assemblies = loadAssemblies();

            List<Type> rocketManagerComponents = getTypesFromParentClass(Assemblies, typeof(RocketManagerComponent));
            /*The API rocketmanagers are loaded in the RocketLauncher, they dont have to be included here*/

            foreach (Type component in rocketManagerComponents)
            {
                Bootstrap.Instance.gameObject.AddComponent(component);
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nLoading Commands".PadRight(80, '.') + "\n");

            SteamGameServer.SetKeyValue("rocket", Assembly.GetExecutingAssembly().GetName().Version.ToString());
            SteamGameServer.SetKeyValue("rocketplugins", String.Join(",", Assemblies.Select(a => a.GetName().Name).ToArray()));
            SteamGameServer.SetKeyValue("maxprotectedslots", RocketPermissionManager.GetProtectedSlots().ToString());
            
            /*But now i could also use the API commands & players loaded */
            Assemblies.Add(Assembly.GetExecutingAssembly());
            /*so i add the rocketapi to Assemblies*/
            List<Type> commands = getTypesFromInterface(Assemblies,"IRocketCommand");
            foreach (Type command in commands)
            {
                IRocketCommand rocketCommand = (IRocketCommand)Activator.CreateInstance(command);
                RocketCommandBase baseCommand = new RocketCommandBase(rocketCommand);
                registerCommand((Command)(baseCommand), command.Assembly.GetName().Name);
            }

            foreach (TextCommand t in RocketSettings.TextCommands)
            {
                registerCommand(new RocketTextCommand(t.Name, t.Help, t.Text));
            }

            //Hacky Hacky :D Commander.Commands = Commander.Commands.Where(c => c.GetType() != typeof(CommandInvestigate)).ToArray();

            SDG.Steam.OnServerConnected += onPlayerConnected;
            SDG.Steam.OnServerDisconnected += onPlayerDisconnected;

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nLaunching Server".PadRight(80, '.'));
            Logger.LogWarning("\nThe error concerning a corrupted file resources.assets can be");
            Logger.LogWarning("ignored while we work on a bugfix".PadRight(79, '.') + "\n");

        }

        internal static void registerCommand(Command command, string originalAssemblyName = null)
        {
            string assemblyName = "";
            if (originalAssemblyName != null)
            {
                assemblyName = originalAssemblyName;
            }
            else
            {
                assemblyName = command.GetType().Assembly.GetName().Name;
            }

            List<Command> commandList = new List<Command>();
            bool msg = false;
            foreach (Command ccommand in Commander.Commands)
            {
                if (ccommand.commandName.ToLower().Equals(command.commandName.ToLower()))
                {
                    if (command is RocketTextCommand) {
                        Logger.LogWarning("Couldn't register RocketTextCommand." + command.commandName + " because it would overwrite " + ccommand.GetType().Assembly.GetName().Name  + "." + ccommand.commandName);
                        return;
                    }
                    if (ccommand.GetType().Assembly.GetName().Name == "Assembly-CSharp")
                    {
                        Logger.LogWarning(assemblyName + "." + command.commandName + " overwrites built in command " + ccommand.commandName);
                        msg = true;
                    }
                    else
                    {
                        Logger.LogWarning("Already register command " + assemblyName + "." + command.commandName + " because it would overwrite " + ccommand.GetType().Assembly.GetName().Name + "." + ccommand.commandName);
                        return;
                    }
                }
                else
                {
                    commandList.Add(ccommand);
                }
            }

            if (command is RocketTextCommand)
            {
                if (!msg) Logger.Log("RocketTextCommand." + command.commandName);
            }
            else
            {
                if (!msg)
                {
                    Logger.Log(assemblyName + "." + command.commandName);
                }
            }
            commandList.Add(command);
            Commander.Commands = commandList.ToArray();
        }

        private void onPlayerConnected(CSteamID id)
        {
            RocketPlayer player = RocketPlayer.FromCSteamID(id);
            List<Type> rocketPlayerComponents = getTypesFromParentClass(Assemblies, typeof(RocketPlayerComponent));
#if DEBUG
            Logger.Log("Adding PlayerComponents");
#endif
            foreach (Type component in rocketPlayerComponents)
            {
#if DEBUG
                Logger.Log(component.Name);
#endif
                player.Player.gameObject.AddComponent(component);
            }

            if (RocketSettings.EnableJoinLeaveMessages)
            {
                RocketChatManager.Say(RocketTranslation.Translate("rocket_join_public", player.CharacterName));
            }
            Rocket.RocketAPI.Events.RocketServerEvents.firePlayerConnected(player);
        }

        private void onPlayerDisconnected(CSteamID id)
        {
            if(RocketSettings.EnableJoinLeaveMessages){
                SDG.Player player = PlayerTool.getPlayer(id);
                RocketChatManager.Say(RocketTranslation.Translate("rocket_leave_public", player.SteamChannel.SteamPlayer.SteamPlayerID.CharacterName));
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
                    Debug.Break();
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

        internal static List<Type> getTypesFromParentClass(List<Assembly> assemblies, Type parentClass)
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

        internal static List<Type> getTypesFromInterface(List<Assembly> assemblies, string interfaceName)
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
                    if (type.GetInterface(interfaceName) != null)
                    {
                        allTypes.Add(type);
                    }
                }
            }
            return allTypes;
        }
    }
}