using Rocket.API;
using Rocket.Core.Logging;
using Rocket.Core.Misc;
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

namespace Rocket.Core.Plugins
{
    public sealed class PluginManager : MonoBehaviour
    {
        private static List<Type> rocketPlayerComponents = new List<Type>();

        private void Start()
        {
#if DEBUG
            Logger.Log("PluginManager > Start");
#endif
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