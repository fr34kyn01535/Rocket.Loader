using Rocket.RocketAPI;
using Rocket.RocketAPI.Events;
using SDG;
using Steamworks;
using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Rocket.Core.Logging;
using Rocket.Core.RCON;
using Rocket.Core.Tasks;

namespace Rocket.Core
{
    public class Rocket : MonoBehaviour
    {
        internal static Rocket Instance;

        public static DateTime Started = DateTime.UtcNow;
        public static float TPS = 0;
        public static string HomeFolder;

        private float updateInterval = 0.005F;
        private float accum = 0;
        private int frames = 0;
        private float timeleft;

        [Browsable(false)]
        public static void Launch()
        {
#if DEBUG
            Console.WriteLine("Launch");
#endif
            Instance = new GameObject().AddComponent<Rocket>();
        }

        [Browsable(false)]
        public static void Splash()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("RocketAPI v" + Assembly.GetExecutingAssembly().GetName().Version.ToString() + " for Unturned v" + Steam.Version + "\n");
            
            Steam.OnServerHosted += () =>
            {
#if DEBUG
                Logger.Log("OnServerHosted"); 
#endif
                Launch();
            };
#if DEBUG
            Console.WriteLine("Splash");
#endif
        }

        private void Start()
        {
#if DEBUG
            Console.WriteLine("Start");
#endif
            Steam.IsServer = true;
            if (String.IsNullOrEmpty(Steam.InstanceName))
            {
                Logger.LogError("Could not get instancename");
                return;
            }
            try
            {
                DontDestroyOnLoad(transform.gameObject);
                HomeFolder = "Servers/" + Steam.InstanceName + "/Rocket/";

                createDirectories();
                moveLibrariesDirectory();
                bindEvents();

                gameObject.AddComponent<TaskManager>();
                gameObject.AddComponent<PluginManager>();
                gameObject.AddComponent<TranslationManager>();
                gameObject.AddComponent<PermissionsManager>();
                gameObject.AddComponent<SettingsManager>();

                launchAutomaticShutdownWatchdog();
                launchRCON();
            }
            catch (Exception e)
            {
                Logger.LogError("Error while loading Rocket: " + e.ToString());
            }
            timeleft = updateInterval;
        }

        private void createDirectories() {
#if DEBUG
                Console.WriteLine("Create directories");
#endif
            if (!Directory.Exists(HomeFolder)) Directory.CreateDirectory(HomeFolder);
            if (!Directory.Exists(HomeFolder + "Plugins/")) Directory.CreateDirectory(HomeFolder + "Plugins/");
            if (!Directory.Exists(HomeFolder + "Libraries/")) Directory.CreateDirectory(HomeFolder + "Libraries/");
            if (!Directory.Exists(HomeFolder + "Logs/")) Directory.CreateDirectory(HomeFolder + "Logs/");
            if (File.Exists(HomeFolder + "Logs/Rocket.log"))
            {
                string ver = ((int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds).ToString();
                File.Move(HomeFolder + "Logs/Rocket.log", HomeFolder + "Logs/Rocket." + ver + ".log");
            };
        }

        private void moveLibrariesDirectory()
        {
            try
            {
                if (Directory.Exists(HomeFolder + "Plugins/Libraries/"))
                {
#if DEBUG
                        Console.WriteLine("Fixing Libraries folder");
#endif
                    foreach (string file in Directory.GetFiles(HomeFolder + "Plugins/Libraries/", "*"))
                    {
                        if (!File.Exists(HomeFolder + "Libraries/" + Path.GetFileName(file)))
                            File.Move(file, HomeFolder + "Libraries/" + Path.GetFileName(file));
                    }
                    Directory.Delete(HomeFolder + "Plugins/Libraries/", true);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
            }
        }

        private void bindEvents() {
            Events.BindEvents();
        }

        private void launchAutomaticShutdownWatchdog()
        {
            if(SettingsManager.AutomaticShutdown.Enabled)
                gameObject.AddComponent<AutomaticShutdownWatchdog>();
        }

        private void launchRCON() {
            if (SettingsManager.RCON.Enabled)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Loading RocketRcon".PadRight(80, '.'));
                int port = SettingsManager.RCON.Port;
                if (SettingsManager.RCON.Minimal)
                {
                    MinimalRconServer.Listen(port);
                }
                else
                {
                    RCONServer.Listen(port);
                }
            }
        }

        private void Update()
        {
            timeleft -= Time.deltaTime;
            accum += Time.timeScale / Time.deltaTime;
            ++frames;

            if (timeleft <= 0.0)
            {
                TPS = accum / frames;

                int left = Console.CursorLeft;
                int top = Console.CursorTop;
                timeleft = updateInterval;
                accum = 0.0F;
                frames = 0;
            }
        }
    }
}