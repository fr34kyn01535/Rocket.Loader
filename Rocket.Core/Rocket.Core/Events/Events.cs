using Rocket.Core.Logging;
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