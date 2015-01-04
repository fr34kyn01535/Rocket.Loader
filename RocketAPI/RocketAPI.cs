using SDG;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;
namespace Rocket
{
    public class RocketAPI : MonoBehaviour
    {
        public static string HomeFolder;

        public static RocketAPI Instance;

        public static void LaunchRocket()
        {
            Instance = new GameObject().AddComponent<RocketAPI>();
        }
        public static void Splash()
        {
            Logger.LogError("".PadRight(80, '.'));
            Logger.LogError(@"                        ______           _        _ ");
            Logger.LogError(@"                        | ___ \         | |      | |");
            Logger.LogError(@"                        | |_/ /___   ___| | _____| |_");
            Logger.LogError(@"                        |    // _ \ / __| |/ / _ \ __|");
            Logger.LogError(@"                        | |\ \ (_) | (__|   <  __/ |_");
            Logger.LogError(@"                        \_| \_\___/ \___|_|\_\___|\__\ v" + Assembly.GetExecutingAssembly().GetName().Version.ToString() + "\n");
            Logger.LogError("Applying configuration".PadRight(80, '.'));
        }

        private void Start()
        {
            if (String.IsNullOrEmpty(Steam.Servername)) return;
            try
            {
                DontDestroyOnLoad(transform.gameObject);
                HomeFolder = "Servers/" + Steam.Servername + "/Rocket/";
               
                if (!Directory.Exists(HomeFolder)) Directory.CreateDirectory(HomeFolder);
                if (!Directory.Exists(RocketAPI.HomeFolder + "Plugins/")) Directory.CreateDirectory(RocketAPI.HomeFolder + "Plugins/");
                if (!Directory.Exists(RocketAPI.HomeFolder + "Plugins/Libraries/")) Directory.CreateDirectory(RocketAPI.HomeFolder + "Plugins/Libraries/");

                gameObject.AddComponent<RocketPluginManager>();
                gameObject.AddComponent<RocketPermissionManager>();
                Logger.LogError("\nGame started".PadRight(80, '.'));
                Logger.LogWarning("[Notice] The following error concerning a corrupted file resourcs.assets can be");
                Logger.LogWarning("ignored while we work on a bugfix".PadRight(79, '.'));
            }
            catch (Exception e)
            {
                Logger.LogError("Error while loading Rocket: " + e.ToString());
            }
        }
    }
}
