using SDG;
using Steamworks;
using UnityEngine;

namespace Rocket
{
    public class RocketComponent : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(transform.gameObject);
            Steam.serverShutdown += onServerShutdown;
            Steam.serverConnected += onPlayerConnected;
            Steam.serverDisconnected += onPlayerDisconnected;
            Load();
        }

        protected virtual void Load() { }

        protected virtual void onPlayerDisconnected(CSteamID cSteamID) { }

        protected virtual void onPlayerConnected(CSteamID cSteamID) { }

        protected virtual void onServerShutdown() { }
    }
}
