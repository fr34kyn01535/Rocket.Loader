using Rocket.API;
using Rocket.Core.Events;
using Rocket.Core.Logging;
using Rocket.Core.Settings;
using Rocket.Unturned.Player;
using Rocket.Unturned.Settings;
using SDG;
using Steamworks;
using System.IO;
using UnityEngine;

namespace Rocket.Unturned.Events
{
    public sealed partial class RocketServerEvents : MonoBehaviour
    {
        private void Awake()
        {
#if DEBUG
            Logger.Log("RocketServerEvents > Awake");
#endif
            RocketEvents.OnRocketSave += () => { SaveManager.save(); };
            RocketEvents.OnRocketAutomaticShutdown += () =>
            {
                if (((ImplementationSettings)RocketSettingsManager.Settings.Implementation).AutoShutdownClearLevel && Directory.Exists(Implementation.Instance.HomeFolder + "../Level/"))
                {
                    Logger.Log("Deleting Level...");
                    Directory.Delete(Implementation.Instance.HomeFolder + "../Level/", true);
                }
                if (((ImplementationSettings)RocketSettingsManager.Settings.Implementation).AutomaticShutdownClearPlayers && Directory.Exists(Implementation.Instance.HomeFolder + "../Players/"))
                {
                    Logger.Log("Deleting Players...");
                    Directory.Delete(Implementation.Instance.HomeFolder + "../Players/", true);
                }
                Logger.Log("Shutting down...");
                SaveManager.save();
                Steam.shutdown();
            };
            RocketEvents.OnRocketCommandTriggered += (string command, ref bool success) => { success = Commander.execute(new Steamworks.CSteamID(0), command); };

            Steam.OnServerShutdown += onServerShutdown;
            Steam.OnServerDisconnected += onPlayerDisconnected;
        }
        
        public delegate void PlayerDisconnected(RocketPlayer player);
        public static event PlayerDisconnected OnPlayerDisconnected;

        private static void onPlayerDisconnected(CSteamID r)
        {
            try
            {
                if (OnPlayerDisconnected != null) OnPlayerDisconnected(RocketPlayer.FromCSteamID(r));
            }
            catch (System.Exception ex)
            {
                Logger.Log(ex);
            }
        }

        public delegate void PlayerConnected(RocketPlayer player);
        public static event PlayerConnected OnPlayerConnected;

        internal static void firePlayerConnected(RocketPlayer player)
        {
            if (OnPlayerConnected != null) OnPlayerConnected(player);
        }

        public delegate void ServerShutdown();
        public static event ServerShutdown OnServerShutdown;

        private static void onServerShutdown()
        {
            try
            {
                if (OnServerShutdown != null) OnServerShutdown();
            }
            catch (System.Exception ex)
            {
                Logger.Log(ex);
            }
        }
    }
}