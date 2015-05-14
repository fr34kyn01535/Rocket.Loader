using Rocket.RocketAPI;
using SDG;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using System.Linq;

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
                RocketChatManager.Say(caller, "Rocket v" + Assembly.GetExecutingAssembly().GetName().Version + " for Unturned v" + Steam.Version);
                RocketChatManager.Say(caller, "https://rocket.foundation © 2015");
                return;
            }

            if (command.Length == 1)
            {
                switch (command[0].ToLower()) {
                    case "plugins":
                        if (caller != null && !caller.HasPermission("rocket.plugins")) return;
                        List<RocketPlugin> plugins = RocketPluginManager.GetPlugins();
                        RocketChatManager.Say(caller, RocketTranslation.Translate("command_rocket_plugins_loaded", String.Join(", ", plugins.Where(p => p.Loaded).Select(p => p.GetType().Assembly.GetName().Name).ToArray())));
                        RocketChatManager.Say(caller, RocketTranslation.Translate("command_rocket_plugins_unloaded", String.Join(", ", plugins.Where(p => !p.Loaded).Select(p => p.GetType().Assembly.GetName().Name).ToArray())));
                        break;
                    case "reload":
                        if (caller != null && !caller.HasPermission("rocket.reload")) return;
                            RocketChatManager.Say(caller, RocketTranslation.Translate("command_rocket_reload"));
                            RocketTranslation.ReloadTranslations();
                            SettingsManager.ReloadSettings();
                            RocketPermissionManager.ReloadPermissions();
                        break;
                }
            }

            if (command.Length == 2)
            {
                RocketPlugin p = RocketPluginManager.GetPlugin(command[1]);
                if (p != null)
                {
                    switch (command[0].ToLower())
                    {
                        case "reload":
                            if (caller != null && !caller.HasPermission("rocket.reloadplugin")) return;
                            if (p.Loaded)
                            {
                                RocketChatManager.Say(caller, RocketTranslation.Translate("command_rocket_reload_plugin", p.GetType().Assembly.GetName().Name));
                                p.UnloadPlugin();
                                p.LoadPlugin();
                            }
                            else
                            {
                                RocketChatManager.Say(caller, RocketTranslation.Translate("command_rocket_not_loaded", p.GetType().Assembly.GetName().Name));
                            }
                            break;
                        case "unload":
                            if (caller != null && !caller.HasPermission("rocket.unloadplugin")) return;
                            if (p.Loaded)
                            {
                                RocketChatManager.Say(caller, RocketTranslation.Translate("command_rocket_unload_plugin", p.GetType().Assembly.GetName().Name));
                                p.UnloadPlugin();
                            }
                            else
                            {
                                RocketChatManager.Say(caller, RocketTranslation.Translate("command_rocket_not_loaded", p.GetType().Assembly.GetName().Name));
                            }
                            break;
                        case "load":
                            if (caller != null && !caller.HasPermission("rocket.loadplugin")) return;
                            if (!p.Loaded)
                            {
                                RocketChatManager.Say(caller, RocketTranslation.Translate("command_rocket_load_plugin", p.GetType().Assembly.GetName().Name));
                                p.LoadPlugin();
                            }
                            else
                            {
                                RocketChatManager.Say(caller, RocketTranslation.Translate("command_rocket_already_loaded", p.GetType().Assembly.GetName().Name));
                            }
                            break;
                    }
                }
                else
                {
                    RocketChatManager.Say(caller, RocketTranslation.Translate("command_rocket_plugin_not_found", command[1]));
                }
            }


        }
    }
}