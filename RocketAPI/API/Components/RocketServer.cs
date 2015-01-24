using Rocket.RocketAPI;
using Rocket.RocketAPI.Components;
using SDG;
using Steamworks;
using UnityEngine;

namespace Rocket.RocketAPI
{
    public class RocketServer : RocketManagerComponent
    {

        private void Start()
        {
            Steam.OnServerShutdown += onServerShutdown;
            Steam.OnServerDisconnected += onPlayerDisconnected;
            Steam.OnServerConnected += onPlayerConnected;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="player">The disconnecting player</param>
        public delegate void PlayerDisconnected(SDG.Player player);
        public static event PlayerDisconnected OnPlayerDisconnected;
        private static void onPlayerDisconnected(CSteamID r)
        {
            try {
                if (OnPlayerDisconnected != null) OnPlayerDisconnected(PlayerTool.getPlayer(r));
            }
            catch (System.Exception ex)
            {
                Logger.Log(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player">The connecting player</param>
        public delegate void PlayerConnected(SDG.Player player);
        public static event PlayerConnected OnPlayerConnected;
        private static void onPlayerConnected(CSteamID r)
        {
            try {
                if (OnPlayerConnected != null) OnPlayerConnected(PlayerTool.getPlayer(r));
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
