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
        public static DateTime Started = DateTime.UtcNow;
        public static float TPS = 0; 
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
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("RocketAPI v" + Assembly.GetExecutingAssembly().GetName().Version.ToString() + " for Unturned v" + Steam.Version + "\n");

#if DEBUG
            Console.WriteLine("Splash");
#else
            //RocketLoadingAnimation.Load();
#endif
        }

        private void Start()
        {
#if DEBUG
            Console.WriteLine("Start");
#endif
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
                if (!Directory.Exists(RocketSettings.HomeFolder + "Logs/")) Directory.CreateDirectory(RocketSettings.HomeFolder + "Logs/");
                if (File.Exists(RocketSettings.HomeFolder + "Logs/Rocket.log"))
                {
                    string ver = ((int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds).ToString();
                    File.Move(RocketSettings.HomeFolder + "Logs/Rocket.log",RocketSettings.HomeFolder + "Logs/Rocket."+ ver +".log");
                };


                /*Cleaning the workspace...*/
                foreach (string file in Directory.GetFiles(RocketSettings.HomeFolder, "*.config", SearchOption.AllDirectories)) {
#if DEBUG
                    Console.WriteLine("Fixing xml files");
#endif
                    if (!File.Exists(file + ".xml"))
                        File.Move(file, file + ".xml");
                }
                try
                {
                if (Directory.Exists(RocketSettings.HomeFolder + "Plugins/Libraries/")) {
#if DEBUG
                Console.WriteLine("Fixing Libraries folder");
#endif
                    foreach (string file in Directory.GetFiles(RocketSettings.HomeFolder + "Plugins/Libraries/", "*"))
                    {
                        if (!File.Exists(RocketSettings.HomeFolder + "Libraries/" + Path.GetFileName(file)))
                            File.Move(file, RocketSettings.HomeFolder + "Libraries/" + Path.GetFileName(file));
                    }
                    Directory.Delete(RocketSettings.HomeFolder + "Plugins/Libraries/", true);
                }

                }
                catch (Exception ex)
                {
                    Logger.LogError(ex.ToString());
                }

#if DEBUG
                Console.WriteLine("LoadSettings");
#endif
                RocketSettings.LoadSettings();
#if DEBUG
                Console.WriteLine("LoadTranslations");
#endif
                RocketTranslation.LoadTranslations();
#if DEBUG
                Console.WriteLine("BindEvents");
#endif
                RocketServerEvents.BindEvents();

                gameObject.AddComponent<RocketTaskManager>();
                gameObject.AddComponent<RocketChatManager>();
                gameObject.AddComponent<RocketPluginManager>();
                gameObject.AddComponent<RocketPermissionManager>();
                gameObject.AddComponent<RocketFeatures>();
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