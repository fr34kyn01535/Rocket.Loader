using Rocket.RocketAPI;
using Rocket.RocketAPI.Managers;
using SDG;
using Steamworks;
using System;
using System.IO;
using UnityEngine;

namespace Rocket
{
    public class RocketLauncher : MonoBehaviour
    {
        public static string HomeFolder;

        public static RocketLauncher Instance;

        public static RocketSettings Settings;

        public float updateInterval = 0.5F;

        public static DateTime Started = DateTime.UtcNow;

        public static float TPS = 0; 
        private float accum = 0;
        private int frames = 0;
        private float timeleft;

        public static float FTPS = 0; 
        private float faccum = 0;
        private int fframes = 0;
        private float ftimeleft; 
        
        public static void Launch()
        {
            Instance = new GameObject().AddComponent<RocketLauncher>();
        }

        public static void Splash()
        {
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
                if (!Directory.Exists(RocketSettings.HomeFolder)) Directory.CreateDirectory(RocketSettings.HomeFolder);
                if (!Directory.Exists(RocketSettings.HomeFolder + "Plugins/")) Directory.CreateDirectory(RocketSettings.HomeFolder + "Plugins/");
                if (!Directory.Exists(RocketSettings.HomeFolder + "Libraries/")) Directory.CreateDirectory(RocketSettings.HomeFolder + "Libraries/");

                RocketSettings.LoadSettings();
                RocketEvents.BindSteamEvents();

                gameObject.AddComponent<RocketTaskManager>();
                gameObject.AddComponent<RocketChatManager>();
                gameObject.AddComponent<RocketPluginManager>();
                gameObject.AddComponent<RocketPermissionManager>();
                gameObject.AddComponent<RocketRconManager>();
            }
            catch (Exception e)
            {
                Logger.LogError("Error while loading Rocket: " + e.ToString());
            }
            timeleft = updateInterval;
            ftimeleft = updateInterval;  
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

        void FixedUpdate()
        {
            ftimeleft -= Time.deltaTime;
            faccum += Time.timeScale / Time.deltaTime;
            ++fframes;

            if (ftimeleft <= 0.0)
            {
                TPS = faccum / fframes;

                int left = Console.CursorLeft;
                int top = Console.CursorTop;
                ftimeleft = updateInterval;
                faccum = 0.0F;
                fframes = 0;
            }
        }
    }
}