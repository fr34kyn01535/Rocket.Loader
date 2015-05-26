﻿using Rocket.API;
using Rocket.Core;
using Rocket.Unturned.Events;
using Rocket.Unturned.Logging;
using Rocket.Unturned.Plugins;
using Rocket.Unturned.Settings;
using SDG;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace Rocket.Unturned
{
    public class Implementation : MonoBehaviour, IRocketImplementation
    {
        public static RocketBootstrap Rocket;
        public static Implementation Instance;

        [Browsable(false)]
        public static void Launch()
        {
            new GameObject().AddComponent<Implementation>();
        }

        [Browsable(false)]
        public static void Splash()
        {
            Console.Clear();
#if DEBUG
            Logger.Log("Implementation > Splash");
#endif
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Rocket Unturned v" + Assembly.GetExecutingAssembly().GetName().Version.ToString() + " for Unturned v" + Steam.Version + "\n");

           // Steam.OnServerHosted += () =>
           // {
                Launch();
            //};
        }

        private void Awake()
        {
#if LINUX
            gameObject.AddComponent(typeof(RocketConsole));
#endif
            Instance = this;
#if DEBUG
            Logger.Log("Implementation > Awake");
#endif
            createDirectories();
            moveLibrariesDirectory();
            
            gameObject.AddComponent(typeof(RocketServerEvents));
            gameObject.AddComponent(typeof(PluginManager));
            
            Rocket = gameObject.AddComponent<RocketBootstrap>();
        }

        public string InstanceName
        {
            get { return Steam.InstanceName; }
        }

        public string HomeFolder { get { return "Servers/" + InstanceName + "/Rocket/"; } }
        public string PluginsFolder { get { return HomeFolder + "Plugins/"; } }
        public string LibrariesFolder { get { return HomeFolder + "Libraries/"; } }
        public string LogsFolder { get { return HomeFolder + "Logs/"; } }
        public string ConfigurationFolder { get { return HomeFolder; } }

        public ImplementationSettingsSection Configuration { get { return new ImplementationSettings(); } }

        public Dictionary<string, string> Translation
        {
            get
            {
                return new Dictionary<string, string>(){
                    {"command_generic_failed_find_player","Failed to find player"},
                    {"command_generic_invalid_parameter","Invalid Parameter"},
                    {"command_generic_target_player_not_found","Target player not found"},
                    {"command_generic_teleport_while_driving_error","You cannot teleport while driving or riding in a vehicle."},
                    {"command_god_enable_console","{0} enabled Godmode"},
                    {"command_god_enable_private","You can feel the strength now..."},
                    {"command_god_disable_console","{0} disabled Godmode"},
                    {"command_god_disable_private","The godly powers left you..."},
                    {"command_vanish_enable_console","{0} enabled Vanishmode"},
                    {"command_vanish_enable_private","You are vanished now..."},
                    {"command_vanish_disable_console","{0} disabled Vanishmode"},
                    {"command_vanish_disable_private","You are no longer vanished..."},

                    {"command_duty_enable_console","{0} is in duty"},
                    {"command_duty_enable_private","You are in duty now..."},
                    {"command_duty_disable_console","{0} is no longer in duty"},
                    {"command_duty_disable_private","You are no longer in duty..."},

                    {"command_bed_no_bed_found_private","You do not have a bed to teleport to."},
                    {"command_i_giving_console","Giving {0} item {1}:{2}"},
                    {"command_i_giving_private","Giving you item {0}x {1} ({2})"},
                    {"command_z_giving_console","Spawning {1} zombies near {0}"},
                    {"command_z_giving_private","Spawning {0} zombies nearby"},
                    {"command_i_giving_failed_private","Failed giving you item {0}x {1} ({2})"},
                    {"command_v_giving_console","Giving {0} vehicle {1}"},
                    {"command_v_giving_private","Giving you a {0} ({1})"},
                    {"command_v_giving_failed_private","Failed giving you a {0} ({1})"},
                    {"command_tps_tps","TPS: {0}"},
                    {"command_tps_running_since","Running since: {0} UTC"},
                    {"command_p_reload_private","Reloaded permissions"},
                    {"command_p_groups_private","{0} groups are: {1}"},
                    {"command_p_permissions_private","{0} permissions are: {1}"},
                    {"command_tp_teleport_console","{0} teleported to {1}"},
                    {"command_tp_teleport_private","Teleported to {0}"},
                    {"command_tp_failed_find_destination","Failed to find destination"},
                    {"command_tphere_teleport_console","{0} was teleported to {1}"},
                    {"command_tphere_teleport_from_private","Teleported {0} to you"},
                    {"command_tphere_teleport_to_private","You were teleported to {0}"},
                    {"command_clear_error","There was an error clearing {0} inventory."},
                    {"command_clear_private","Your inventory was cleared!"},
                    {"command_clear_other","Your inventory was cleared by {0}!"},
                    {"command_clear_other_success","You successfully cleared {0} inventory."},
                    {"command_investigate_private","{0} SteamID64 is {1}"},
                    {"command_heal_success_me","{0} was successfully healed"},
                    {"command_heal_success_other","You were healed by {0}"},
                    {"command_heal_success","You were healed"},
                    {"command_compass_facing_private","You are facing {0}"},
                    {"command_compass_north","N"},
                    {"command_compass_east","E"},
                    {"command_compass_south","S"},
                    {"command_compass_west","W"},
                    {"command_compass_northwest","NW"},
                    {"command_compass_northeast","NE"},
                    {"command_compass_southwest","SW"},
                    {"command_compass_southeast","SE"},
                    {"command_rocket_plugins_loaded","Plugins loaded: {0}"},
                    {"command_rocket_plugins_unloaded","Plugins unloaded: {0}"},
                    {"command_rocket_reload_plugin","Reloading {0}"},
                    {"command_rocket_not_loaded","The plugin {0} is not loaded"},
                    {"command_rocket_unload_plugin","Unloading {0}"},
                    {"command_rocket_load_plugin","Loading {0}"},
                    {"command_rocket_already_loaded","The plugin {0} is already loaded"},
                    {"command_rocket_reload","Reloading Rocket"},
                    {"command_rocket_plugin_not_found","Plugin {0} not found"}
                };
            }
        }

        private void createDirectories()
        {
            if (!Directory.Exists(HomeFolder)) Directory.CreateDirectory(HomeFolder);
            if (!Directory.Exists(PluginsFolder)) Directory.CreateDirectory(PluginsFolder);
            if (!Directory.Exists(LibrariesFolder)) Directory.CreateDirectory(LibrariesFolder);
            if (!Directory.Exists(LogsFolder)) Directory.CreateDirectory(LogsFolder);
            if (File.Exists(LogsFolder + "Rocket.log"))
            {
                string ver = ((int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds).ToString();
                File.Move(LogsFolder + "Rocket.log", LogsFolder + "Rocket." + ver + ".log");
            };
        }

        private void moveLibrariesDirectory()
        {
            try
            {
                if (Directory.Exists(PluginsFolder + "Libraries/"))
                {
                    foreach (string file in Directory.GetFiles(PluginsFolder+"Libraries/", "*"))
                    {
                        if (!File.Exists(LibrariesFolder + Path.GetFileName(file)))
                            File.Move(file, LibrariesFolder + Path.GetFileName(file));
                    }
                    Directory.Delete(PluginsFolder + "Libraries/", true);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
            }
        }
    }
}
