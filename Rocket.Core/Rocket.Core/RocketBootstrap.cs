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
using Rocket.API;
using Rocket.Core.Settings;
using Rocket.Core.Plugins;
using Rocket.Core.Translations;
using Rocket.Core.Permissions;
using Rocket.Core.Misc;

namespace Rocket.Core
{
    public class RocketBootstrap : MonoBehaviour
    {
        internal static RocketBootstrap Instance;
        internal static IRocketImplementation Implementation;

        public static DateTime Started = DateTime.UtcNow;
        public static float TPS = 0;

        private float updateInterval = 0.005F;
        private float accum = 0;
        private int frames = 0;
        private float timeleft;

        private void Awake()
        {
#if DEBUG
            Logger.Log("RocketBootstrap > Awake");
#endif
            try
            {
                Instance = this;
                Implementation = (IRocketImplementation)GetComponent(typeof(IRocketImplementation));

                gameObject.AddComponent<RocketTaskManager>();
                gameObject.AddComponent<RocketPluginManager>();
                gameObject.AddComponent<RocketTranslationManager>();
                gameObject.AddComponent<RocketPermissionsManager>();
                gameObject.AddComponent<RocketSettingsManager>();
                gameObject.AddComponent<AutomaticShutdownWatchdog>();
            }
            catch (Exception e)
            {
                Logger.LogError("Error while loading Rocket: " + e.ToString());
            }
        }

        private void Start(){
#if DEBUG
            Logger.Log("RocketBootstrap > Start");
#endif
            try
            {
                launchAutomaticShutdownWatchdog();
                launchRCON();
            }
            catch (Exception e)
            {
                Logger.LogError("Error while loading Rocket: " + e.ToString());
            }
            timeleft = updateInterval;
        }

        private void launchAutomaticShutdownWatchdog()
        {
            if(RocketSettingsManager.Settings.AutomaticShutdown.Enabled)
                gameObject.AddComponent<AutomaticShutdownWatchdog>();
        }

        private void launchRCON() {
            if (RocketSettingsManager.Settings.RCON.Enabled)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Loading RocketRcon".PadRight(80, '.'));
                int port = RocketSettingsManager.Settings.RCON.Port;
                if (RocketSettingsManager.Settings.RCON.Minimal)
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