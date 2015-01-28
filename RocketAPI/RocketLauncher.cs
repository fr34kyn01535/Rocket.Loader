using Rocket.RocketAPI;
using Rocket.RocketAPI.Managers;
using SDG;
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

        public static void Launch()
        {
            Instance = new GameObject().AddComponent<RocketLauncher>();
        }

        public static void Splash()
        {
            RocketLoadingAnimation.Load();
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
                RocketEvents.BindEvents();

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
        }
    }
}