using Rocket.API;
using Rocket.Core.Logging;
using Rocket.Core.Misc;
using Rocket.Core.Plugins;
using Rocket.Core.Settings;
using Rocket.Core.Translations;
using Rocket.Unturned;
using Rocket.Unturned.Commands;
using Rocket.Unturned.Player;
using Rocket.Unturned.Settings;
using SDG;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Rocket.Unturned.Plugins
{
    public sealed class PluginManager : MonoBehaviour
    {
        private static List<Type> rocketPlayerComponents = new List<Type>();

        private void Start()
        {
#if DEBUG
            Logger.Log("PluginManager > Start");
#endif
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(("Loading Rocket.Unturned " + Assembly.GetExecutingAssembly().GetName().Version.ToString()).PadRight(80, '.'));
            Console.ForegroundColor = ConsoleColor.White;
            AddRocketPlayerComponents(Assembly.GetExecutingAssembly());
            RegisterCommands(Assembly.GetExecutingAssembly());

            SDG.Steam.OnServerConnected += onPlayerConnected;
            SDG.Steam.OnServerDisconnected += onPlayerDisconnected;

            SteamGameServer.SetKeyValue("rocket", Assembly.GetExecutingAssembly().GetName().Version.ToString());
            SteamGameServer.SetKeyValue("rocketplugins", String.Join(",", RocketPluginManager.GetPluginNames()));
            SteamGameServer.SetKeyValue("maxprotectedslots", ((ImplementationSettings)RocketSettingsManager.Settings.Implementation).ReservedSlots.ToString());
            SteamGameServer.SetBotPlayerCount(1);
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
                Console.ForegroundColor = ConsoleColor.Cyan;
                List<Type> playerComponents = RocketHelper.GetTypesFromParentClass(plugin, typeof(RocketPlayerComponent));
                rocketPlayerComponents.AddRange(playerComponents);
                Console.WriteLine("     Loading " + playerComponents.Count+" RocketPlayerComponents");
                foreach (Type playerComponent in playerComponents)
                {
                    Steam.Players.ForEach(p => p.Player.gameObject.AddComponent(playerComponent.GetType()));
                }
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
            foreach(Command c in Commander.Commands.Where(c => (c is RocketCommandBase && ((RocketCommandBase)c).Command.GetType().Assembly == assembly)).ToList()){
                Commander.deregister(c);
            }
        }

		private static void RegisterCommand(Command command)
		{
			RegisterCommand(command,null);
		}

        private static void RegisterCommand(Command command, string originalAssemblyName)
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

            List<Command> commandList = Commander.Commands.ToList();
            List<Command> filteredCommandList = commandList.Where(c => c.commandName.ToLower() != command.commandName.ToLower()).ToList();



            if (commandList.Count() != filteredCommandList.Count())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("     ~ /" +command.commandName +" - " +command.commandHelp);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("     + /" + command.commandName + " - " + command.commandHelp);
            }

            Console.ForegroundColor = ConsoleColor.White;

            Commander.register(command);
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

            if (RocketSettingsManager.Settings.EnableJoinLeaveMessages)
            {
                Rocket.Unturned.RocketChat.Say(RocketTranslationManager.Translate("rocket_join_public", player.CharacterName));
            }
        }

        private void onPlayerDisconnected(CSteamID id)
        {
            if(RocketSettingsManager.Settings.EnableJoinLeaveMessages){
                SDG.Player player = PlayerTool.getPlayer(id);
                Rocket.Unturned.RocketChat.Say(RocketTranslationManager.Translate("rocket_leave_public", player.SteamChannel.SteamPlayer.SteamPlayerID.CharacterName));
            }
        }
    }
}