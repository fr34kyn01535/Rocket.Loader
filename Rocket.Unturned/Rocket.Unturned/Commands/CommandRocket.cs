using SDG;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using System.Linq;
using Rocket.API;
using Rocket.Core;
using Rocket.Core.Settings;
using Rocket.Core.Plugins;
using Rocket.Core.Permissions;
using Rocket.Core.Translations;
using Rocket.Unturned.Plugins;
using Rocket.Unturned.Player;

namespace Rocket.Unturned.Commands
{
    public class CommandRocket : IRocketCommand
    {
        public bool RunFromConsole
        {
            get { return true; }
        }

        public string Name
        {
            get { return "rocket"; }
        }

        public string Help
        {
            get { return "About us :)";}
        }

        public void Execute(RocketPlayer caller, string[] command)
        {
            if (command.Length == 0)
            {
                RocketChat.Say(caller, "Rocket v" + Assembly.GetExecutingAssembly().GetName().Version + " for Unturned v" + Steam.Version);
                RocketChat.Say(caller, "https://rocket.foundation © 2015");
                return;
            }

            if (command.Length == 1)
            {
                switch (command[0].ToLower()) {
                    case "plugins":
                        if (caller != null && !caller.HasPermission("rocket.plugins")) return;
                        List<IRocketPlugin> plugins = RocketPluginManager.GetPlugins();
                        RocketChat.Say(caller, RocketTranslationManager.Translate("command_rocket_plugins_loaded", String.Join(", ", plugins.Where(p => p.Loaded).Select(p => p.GetType().Assembly.GetName().Name).ToArray())));
                        RocketChat.Say(caller, RocketTranslationManager.Translate("command_rocket_plugins_unloaded", String.Join(", ", plugins.Where(p => !p.Loaded).Select(p => p.GetType().Assembly.GetName().Name).ToArray())));
                        break;
                    case "reload":
                        if (caller != null && !caller.HasPermission("rocket.reload")) return;
                            RocketChat.Say(caller, RocketTranslationManager.Translate("command_rocket_reload"));
                            RocketTranslationManager.Reload();
                            RocketSettingsManager.Reload();
                            RocketPermissionsManager.Reload();
                        break;
                }
            }

            if (command.Length == 2)
            {
                RocketPlugin p = (RocketPlugin)RocketPluginManager.GetPlugin(command[1]);
                if (p != null)
                {
                    switch (command[0].ToLower())
                    {
                        case "reload":
                            if (caller != null && !caller.HasPermission("rocket.reloadplugin")) return;
                            if (p.Loaded)
                            {
                                RocketChat.Say(caller, RocketTranslationManager.Translate("command_rocket_reload_plugin", p.GetType().Assembly.GetName().Name));
                                p.UnloadPlugin();
                                p.LoadPlugin();
                            }
                            else
                            {
                                RocketChat.Say(caller, RocketTranslationManager.Translate("command_rocket_not_loaded", p.GetType().Assembly.GetName().Name));
                            }
                            break;
                        case "unload":
                            if (caller != null && !caller.HasPermission("rocket.unloadplugin")) return;
                            if (p.Loaded)
                            {
                                RocketChat.Say(caller, RocketTranslationManager.Translate("command_rocket_unload_plugin", p.GetType().Assembly.GetName().Name));
                                p.UnloadPlugin();
                            }
                            else
                            {
                                RocketChat.Say(caller, RocketTranslationManager.Translate("command_rocket_not_loaded", p.GetType().Assembly.GetName().Name));
                            }
                            break;
                        case "load":
                            if (caller != null && !caller.HasPermission("rocket.loadplugin")) return;
                            if (!p.Loaded)
                            {
                                RocketChat.Say(caller, RocketTranslationManager.Translate("command_rocket_load_plugin", p.GetType().Assembly.GetName().Name));
                                p.LoadPlugin();
                            }
                            else
                            {
                                RocketChat.Say(caller, RocketTranslationManager.Translate("command_rocket_already_loaded", p.GetType().Assembly.GetName().Name));
                            }
                            break;
                    }
                }
                else
                {
                    RocketChat.Say(caller, RocketTranslationManager.Translate("command_rocket_plugin_not_found", command[1]));
                }
            }


        }
    }
}