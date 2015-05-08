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

            if (command.Length == 1) {
                switch (command[0].ToLower()) {
                    case "plugins":
                        List<RocketPlugin> plugins = RocketPluginManager.GetPlugins();
                        RocketChatManager.Say(caller, "Plugins loaded: " + String.Join(",", plugins.Where(p => p.Loaded).Select(p => p.GetType().Assembly.GetName().Name).ToArray()));
                        RocketChatManager.Say(caller, "Plugins unloaded: " + String.Join(",", plugins.Where(p => !p.Loaded).Select(p => p.GetType().Assembly.GetName().Name).ToArray()));
                        break;
                }
            }

            if (command.Length == 2) {
                switch (command[0].ToLower())
                {
                    case "reload":
                        RocketPlugin p1 = RocketPluginManager.GetPlugin(command[1]);
                        if (p1 != null && p1.Loaded)
                        {
                            p1.UnloadPlugin();
                            p1.LoadPlugin();
                        }
                        break;
                    case "unload":
                        RocketPlugin p2 = RocketPluginManager.GetPlugin(command[1]);
                        if (p2 != null && p2.Loaded)
                        {
                            p2.UnloadPlugin();
                        }
                        break;
                    case "load":
                        RocketPlugin p3 = RocketPluginManager.GetPlugin(command[1]);
                        if (p3 != null && !p3.Loaded)
                        {
                            p3.LoadPlugin();
                        }
                        break;
                }
            
            }


        }
    }
}