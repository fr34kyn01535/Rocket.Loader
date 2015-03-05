using Rocket.Logging;
using Rocket.Rcon;
using Rocket.RocketAPI;
using Rocket.RocketAPI.Events;
using SDG;
using Steamworks;
using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace Rocket
{
    public class Bootstrap : MonoBehaviour
    {
        internal static Bootstrap Instance;

        private float updateInterval = 0.005F;
        internal static DateTime Started = DateTime.UtcNow;
        internal static float TPS = 0; 
        private float accum = 0;
        private int frames = 0;
        private float timeleft;

        [Browsable(false)]
        public static void Launch()
        {
#if DEBUG
            Console.WriteLine("Launch");
#endif
            Instance = new GameObject().AddComponent<Bootstrap>();
        }

        [Browsable(false)]
        public static void Splash()
        {
#if DEBUG
            Console.WriteLine("Splash");
#else
            //RocketLoadingAnimation.Load();
#endif
        }

        private void Start()
        {
            Console.WriteLine("Start");
            if (String.IsNullOrEmpty(Steam.InstanceName))
            {
                Logger.LogError("Could not get instancename");
                return;
            }
            try
            {
                DontDestroyOnLoad(transform.gameObject);
                RocketSettings.HomeFolder = "Servers/" + Steam.InstanceName + "/Rocket/";
#if DEBUG
                Console.WriteLine("Create directories");
#endif
                if (!Directory.Exists(RocketSettings.HomeFolder)) Directory.CreateDirectory(RocketSettings.HomeFolder);
                if (!Directory.Exists(RocketSettings.HomeFolder + "Plugins/")) Directory.CreateDirectory(RocketSettings.HomeFolder + "Plugins/");
                if (!Directory.Exists(RocketSettings.HomeFolder + "Libraries/")) Directory.CreateDirectory(RocketSettings.HomeFolder + "Libraries/");

#if DEBUG
                Console.WriteLine("LoadSettings");
#endif
                RocketSettings.LoadSettings();
#if DEBUG
                Console.WriteLine("BindEvents");
#endif
                RocketServerEvents.BindEvents();

                gameObject.AddComponent<RocketTaskManager>();
                gameObject.AddComponent<RocketChatManager>();
                gameObject.AddComponent<RocketPluginManager>();
                gameObject.AddComponent<RocketPermissionManager>();
            }
            catch (Exception e)
            {
                Logger.LogError("Error while loading Rocket: " + e.ToString());
            }
            timeleft = updateInterval;
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