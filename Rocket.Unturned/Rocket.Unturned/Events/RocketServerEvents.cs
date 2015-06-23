﻿using Rocket.API;
using Rocket.Core.Events;
using Rocket.Core.Logging;
using Rocket.Core.Settings;
using Rocket.Unturned.Player;
using Rocket.Unturned.Settings;
using SDG.Unturned;
using Steamworks;
using System.IO;
using UnityEngine;
using System.Linq;
using System;

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
            RocketEvents.TryTrigger<PlayerDisconnected>(OnPlayerDisconnected,RocketPlayer.FromCSteamID(r));
        }

        public delegate void PlayerConnected(RocketPlayer player);
        public static event PlayerConnected OnPlayerConnected;

        internal static void firePlayerConnected(RocketPlayer player)
        {
            RocketEvents.TryTrigger<PlayerConnected>(OnPlayerConnected, player);
        }

        public delegate void PlayerConnect(CSteamID player, ref ESteamRejection? rejection);
        public static event PlayerConnect OnPlayerConnect;

        internal static void firePlayerConnect(CSteamID player,ref ESteamRejection? rejection)
        {
            if (OnPlayerConnect != null)
            {
                foreach (var handler in OnPlayerConnect.GetInvocationList().Cast<PlayerConnect>())
                {
                    try
                    {
                        handler(player, ref rejection);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogException(ex);
                    }
                }
            }
        }

        public delegate void ServerShutdown();
        public static event ServerShutdown OnServerShutdown;

        private static void onServerShutdown()
        {
            RocketEvents.TryTrigger<ServerShutdown>(OnServerShutdown);
        }
    }
}