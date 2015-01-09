using Rocket.RocketAPI;
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

        public static void Launch()
        {
            Instance = new GameObject().AddComponent<RocketLauncher>();
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
            Logger.LogError("Loading Unturned".PadRight(80, '.'));
        }

        private void Start()
        {
            if (String.IsNullOrEmpty(Steam.Servername)) return;
            try
            {
                DontDestroyOnLoad(transform.gameObject);
                RocketSettings.HomeFolder = "Servers/" + Steam.Servername + "/Rocket/";

                if (!Directory.Exists(RocketSettings.HomeFolder)) Directory.CreateDirectory(RocketSettings.HomeFolder);
                if (!Directory.Exists(RocketSettings.HomeFolder + "Plugins/")) Directory.CreateDirectory(RocketSettings.HomeFolder + "Plugins/");
                if (!Directory.Exists(RocketSettings.HomeFolder + "Plugins/Libraries/")) Directory.CreateDirectory(RocketSettings.HomeFolder + "Plugins/Libraries/");

                gameObject.AddComponent<RocketManager>();
                gameObject.AddComponent<RocketPluginManager>();
                gameObject.AddComponent<RocketPermissionManager>();
                gameObject.AddComponent<RocketChatManager>();

                Logger.LogError("\nLaunching Unturned".PadRight(80, '.'));
                Logger.LogWarning("The error concerning a corrupted file resourcs.assets can be");
                Logger.LogWarning("ignored while we work on a bugfix".PadRight(79, '.'));
            }
            catch (Exception e)
            {
                Logger.LogError("Error while loading Rocket: " + e.ToString());
            }
        }
    }
}

internal static class RocketSettings
{
    public static string HomeFolder;
}