using Rocket.Logging;
using Rocket.Rcon;
using Rocket.RocketAPI;
using Rocket.RocketAPI.Events;
using SDG;
using Steamworks;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace Rocket
{
    public class RocketLauncher : MonoBehaviour
    {
        public static string HomeFolder;

        public static RocketLauncher Instance;

        public static RocketSettings Settings;

        public float updateInterval = 0.005F;

        public static DateTime Started = DateTime.UtcNow;

        public static float TPS = 0; 
        private float accum = 0;
        private int frames = 0;
        private float timeleft;
        
        public static void Launch()
        {
            Instance = new GameObject().AddComponent<RocketLauncher>();
        }

        public static void Splash()
        {
          /*  try
            {
               // Pipe = new RocketNamedPipe(Steam.InstanceName,"GAME", "LAUNCHER");
               // Pipe.OnMessage += PipeHandler.Handle;
            }
            catch (TypeLoadException ex)
            {
                Console.WriteLine("dafq");
                Console.WriteLine(ex.TypeName);
                Console.WriteLine(ex.Source);
                //Console.WriteLine(ex.Data);
                Console.WriteLine(ex.ToString());
            }*/
#if !DEBUG
            RocketLoadingAnimation.Load();
#endif
        }

        private void Start()
        {
            if (String.IsNullOrEmpty(Steam.InstanceName)) return;
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
        void Update()
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