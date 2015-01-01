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
using Rocket.RocketAPI.Managers;


namespace Rocket.RocketAPI
{
    /// <summary>
    /// This is the core class for the RocketAPI
    /// </summary>
    public class RocketAPI
    {
        /// <summary>
        /// CommandManager
        /// </summary>
        public static CommandManager Commands;
        /// <summary>
        /// PermissionManager
        /// </summary>
        public static PermissionManager Permissions;
        /// <summary>
        /// PluginManager
        /// </summary>
        public static PluginManager Plugins;
        /// <summary>
        /// EventManager
        /// </summary>
        public static EventManager Events;

        internal RocketAPI()
        {
            Logger.LogError("".PadLeft(80, '.'));
            Logger.LogError(@"                        ______           _        _ ");
            Logger.LogError(@"                        | ___ \         | |      | |");
            Logger.LogError(@"                        | |_/ /___   ___| | _____| |_");
            Logger.LogError(@"                        |    // _ \ / __| |/ / _ \ __|");
            Logger.LogError(@"                        | |\ \ (_) | (__|   <  __/ |_");
            Logger.LogError(@"                        \_| \_\___/ \___|_|\_\___|\__\ v" + Assembly.GetExecutingAssembly().GetName().Version.ToString() + "\n");
            Logger.LogError("".PadLeft(80, '.'));

            Commands = new CommandManager();
            Permissions = new PermissionManager();
            Plugins = new PluginManager();
            Events = new EventManager();
        }

        internal void Reload()
        {
            Commands.Reload();
            Permissions.Reload();
            Plugins.Reload();
            Events.Reload();
        }
    }

}
