using SDG;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;
namespace Rocket
{
    public class RocketAPI : MonoBehaviour
    {
        public static string HomeFolder = "";

        public static RocketAPI Instance;

        public static void LaunchRocket()
        {
            Instance = new GameObject().AddComponent<RocketAPI>();
        }

        private void Awake()
        {
            try
            {
                DontDestroyOnLoad(transform.gameObject);
                Logger.LogError("".PadLeft(80, '.'));
                Logger.LogError(@"                        ______           _        _ ");
                Logger.LogError(@"                        | ___ \         | |      | |");
                Logger.LogError(@"                        | |_/ /___   ___| | _____| |_");
                Logger.LogError(@"                        |    // _ \ / __| |/ / _ \ __|");
                Logger.LogError(@"                        | |\ \ (_) | (__|   <  __/ |_");
                Logger.LogError(@"                        \_| \_\___/ \___|_|\_\___|\__\ v" + Assembly.GetExecutingAssembly().GetName().Version.ToString() + "\n");
                Logger.LogError("".PadLeft(80, '.'));
                HomeFolder = "Servers/" + Steam.Servername + "/Rocket/";
               
                if (!Directory.Exists(HomeFolder)) Directory.CreateDirectory(HomeFolder);
                if (!Directory.Exists(RocketAPI.HomeFolder + "Plugins/")) Directory.CreateDirectory(RocketAPI.HomeFolder + "Plugins/");
                if (!Directory.Exists(RocketAPI.HomeFolder + "Plugins/Libraries/")) Directory.CreateDirectory(RocketAPI.HomeFolder + "Plugins/Libraries/");

                gameObject.AddComponent<RocketPluginManager>();
                gameObject.AddComponent<RocketPermissionManager>();
            }
            catch (Exception e)
            {
                Logger.LogError("Error while loading Rocket: " + e.ToString());
            }
        }
    }
}
