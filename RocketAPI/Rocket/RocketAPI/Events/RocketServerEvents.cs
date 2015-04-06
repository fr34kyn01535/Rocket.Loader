using Rocket.Logging;
using SDG;
using Steamworks;
using UnityEngine;

namespace Rocket.RocketAPI.Events
{
    public sealed partial class RocketServerEvents
    {
        public static void BindEvents()
        {
            Steam.OnServerShutdown += onServerShutdown;
            Steam.OnServerDisconnected += onPlayerDisconnected;
            Steam.OnServerConnected += onPlayerConnected;
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

        private static void onPlayerConnected(CSteamID r)
        {
            try
            {
                if (OnPlayerConnected != null) OnPlayerConnected(RocketPlayer.FromCSteamID(r));
            }
            catch (System.Exception ex)
            {
                Logger.Log(ex);
            }
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