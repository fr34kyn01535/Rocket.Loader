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
        private static List<Assembly> pluginAssemblies;
        private Dictionary<string, string> additionalLibraries = new Dictionary<string, string>();
        private static List<Type> rocketPlayerComponents = new List<Type>();

        public static RocketPlugin GetPlugin(string name) {
            Assembly assembly = pluginAssemblies.Where(a => a.GetName().Name.ToLower().Contains(name.ToLower())).FirstOrDefault();
            if (assembly == null) return null;

            Type plugin = RocketHelper.GetTypesFromParentClass(assembly, typeof(RocketPlugin)).FirstOrDefault();

            return GetPlugin(plugin);
        }

        public static RocketPlugin GetPlugin(Type plugin)
        {
            Component c = Bootstrap.Instance.gameObject.GetComponent(plugin);
            return (c is RocketPlugin) ? (RocketPlugin)c : null;
        }

        public static string[] GetPluginNames() {
            return pluginAssemblies.Select(a => a.GetName().Name).ToArray();
        }

        public static List<RocketPlugin> GetPlugins()
        {
            return pluginAssemblies.Select(a => GetPlugin(a.GetName().Name)).ToList();
        }

        private void Start()
        {
#if DEBUG
            Logger.Log("Start RocketPluginManager");
#endif
            rocketPlayerComponents = RocketHelper.GetTypesFromParentClass(Assembly.GetExecutingAssembly(), typeof(RocketPlayerComponent));
            RegisterCommands(Assembly.GetExecutingAssembly());

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

            additionalLibraries = loadAdditionalAssemblies();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine();
            Console.WriteLine("Loading Plugins".PadRight(80, '.'));
            pluginAssemblies = loadPluginAssemblies();
            List<Type> rocketManagerComponents = RocketHelper.GetTypesFromParentClass(pluginAssemblies, typeof(RocketPlugin));

            foreach (Type component in rocketManagerComponents)
            {
                Bootstrap.Instance.gameObject.AddComponent(component);
            }

            SteamGameServer.SetKeyValue("rocket", Assembly.GetExecutingAssembly().GetName().Version.ToString());
            SteamGameServer.SetKeyValue("rocketplugins", String.Join(",", pluginAssemblies.Select(a => a.GetName().Name).ToArray()));
            SteamGameServer.SetKeyValue("maxprotectedslots", RocketPermissionManager.GetProtectedSlots().ToString());

            SteamGameServer.SetBotPlayerCount(1);

            SDG.Steam.OnServerConnected += onPlayerConnected;
            SDG.Steam.OnServerDisconnected += onPlayerDisconnected;

            Console.ForegroundColor = ConsoleColor.Cyan;

        }

        public void load() { 
            
        }

        public void unload() {
        }

        internal void Reload() { 
            unload();
            load();
        }

        internal static void RemoveRocketPlayerComponents(Assembly plugin)
        {
            try
            {
                rocketPlayerComponents = rocketPlayerComponents.Where(p => p.Assembly != plugin).ToList();
                List<Type> playerComponents = RocketHelper.GetTypesFromParentClass(plugin, typeof(RocketPlayerComponent));
                Steam.Players.ForEach(p => Destroy(p.Player.gameObject.GetComponent(playerComponents.GetType())));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        internal static void AddRocketPlayerComponents(Assembly plugin)
        {
            try
            {
                List<Type> playerComponents = RocketHelper.GetTypesFromParentClass(plugin, typeof(RocketPlayerComponent));
                rocketPlayerComponents.AddRange(playerComponents);
                Steam.Players.ForEach(p => p.Player.gameObject.AddComponent(playerComponents.GetType()));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        internal static void RegisterCommands(Assembly assembly)
        {
            List<Type> commands = RocketHelper.GetTypesFromInterface(assembly, "IRocketCommand");
            foreach (Type command in commands)
            {
                IRocketCommand rocketCommand = (IRocketCommand)Activator.CreateInstance(command);
                RocketCommandBase baseCommand = new RocketCommandBase(rocketCommand);
                RegisterCommand((Command)(baseCommand), command.Assembly.GetName().Name);
            }
        }

        internal static void UnregisterCommands(Assembly assembly)
        {
            Commander.Commands = Commander.Commands.Where(c => !(c is RocketCommandBase) || (c is RocketCommandBase && ((RocketCommandBase)c).Command.GetType().Assembly != assembly)).ToArray();
        }

        private static void RegisterCommand(Command command, string originalAssemblyName = null)
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
            foreach (Command ccommand in Commander.Commands)
            {
                if (ccommand.commandName.ToLower().Equals(command.commandName.ToLower()))
                {
                    if (ccommand.GetType().Assembly.GetName().Name == "Assembly-CSharp")
                    {
                        Logger.LogWarning(assemblyName + "." + command.commandName + " overwrites built in command " + ccommand.commandName);
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
            commandList.Add(command);
            Commander.Commands = commandList.ToArray();
        }

        private void onPlayerConnected(CSteamID id)
        {
            RocketPlayer player = RocketPlayer.FromCSteamID(id);
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

        private Dictionary<string, string> loadAdditionalAssemblies()
        {
            Dictionary<string, string> l = new Dictionary<string, string>(); 
            IEnumerable<FileInfo> libraries = new DirectoryInfo(RocketSettings.HomeFolder + "Libraries/").GetFiles("*.dll", SearchOption.AllDirectories).Where(f => f.Extension == ".dll");
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
     
        private static List<Assembly> loadPluginAssemblies()
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
    }
}