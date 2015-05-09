using Rocket.Logging;
using Rocket.RocketAPI;
using SDG;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using System.Linq;

namespace Rocket.Commands
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
                        if (caller != null && !(caller.HasPermission("rocket.plugins") || caller.HasPermission("rocket.*"))) return;
                        List<RocketPlugin> plugins = RocketPluginManager.GetPlugins();
                        RocketChatManager.Say(caller, RocketTranslation.Translate("command_rocket_plugins_loaded", String.Join(", ", plugins.Where(p => p.Loaded).Select(p => p.GetType().Assembly.GetName().Name).ToArray())));
                        RocketChatManager.Say(caller, RocketTranslation.Translate("command_rocket_plugins_unloaded", String.Join(", ", plugins.Where(p => !p.Loaded).Select(p => p.GetType().Assembly.GetName().Name).ToArray())));
                        break;
                    case "reload":
                        if (caller != null && !(caller.HasPermission("rocket.reload") || caller.HasPermission("rocket.*"))) return;
                            RocketPermissionManager.ReloadPermissions();
                            RocketTranslation.LoadTranslations();
                            RocketSettings.LoadSettings();
                            RocketChatManager.Say(caller, RocketTranslation.Translate("command_rocket_reload"));
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
                            if (caller != null && !(caller.HasPermission("rocket.reloadplugin") || !caller.HasPermission("rocket.*"))) return;
                            if (p.Loaded)
                            {
                                p.UnloadPlugin();
                                p.LoadPlugin();
                                RocketChatManager.Say(caller, RocketTranslation.Translate("command_rocket_reload_plugin", p.GetType().Assembly.GetName().Name));
                            }
                            else
                            {
                                RocketChatManager.Say(caller, RocketTranslation.Translate("command_rocket_not_loaded", p.GetType().Assembly.GetName().Name));
                            }
                            break;
                        case "unload":
                            if (caller != null && !(caller.HasPermission("rocket.unloadplugin") || caller.HasPermission("rocket.*"))) return;
                            if (p.Loaded)
                            {
                                p.UnloadPlugin();
                                RocketChatManager.Say(caller, RocketTranslation.Translate("command_rocket_unload_plugin", p.GetType().Assembly.GetName().Name));
                            }
                            else
                            {
                                RocketChatManager.Say(caller, RocketTranslation.Translate("command_rocket_not_loaded", p.GetType().Assembly.GetName().Name));
                            }
                            break;
                        case "load":
                            if (caller != null && !(caller.HasPermission("rocket.loadplugin") || caller.HasPermission("rocket.*"))) return;
                            if (!p.Loaded)
                            {
                                p.LoadPlugin();
                                RocketChatManager.Say(caller, RocketTranslation.Translate("command_rocket_load_plugin", p.GetType().Assembly.GetName().Name));
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