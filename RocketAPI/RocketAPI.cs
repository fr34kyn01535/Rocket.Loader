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
    public class RocketAPI
    {
        public static CommandManager Commands;
        public static PermissionManager Permissions;
        public static PluginManager Plugins;
        public static EventManager Events;

        public RocketAPI()
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

        public void Reload()
        {
            Commands.Reload();
            Permissions.Reload();
            Plugins.Reload();
            Events.Reload();
        }
    }

}
