using Rocket.Core.Logging;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using UnityEngine;
using System.Linq;
using System;
using Rocket.Core.Extensions;
using Rocket.API;

namespace Rocket.Unturned.Events
{
    public sealed class ImplementationEvents : MonoBehaviour, IRocketImplementationEvents
    {
        private void Awake()
        {
            Steam.OnServerDisconnected += (CSteamID r) => { OnPlayerDisconnected.TryInvoke(UnturnedPlayer.FromCSteamID(r)); };
            Steam.OnServerShutdown += () => { onShutdown.TryInvoke(); };
        }

        public delegate void PlayerDisconnected(UnturnedPlayer player);
        public event PlayerDisconnected OnPlayerDisconnected;

        private event ImplementationShutdown onShutdown;
        public event ImplementationShutdown OnShutdown
        {
            add
            {
                onShutdown += value;
            }

            remove
            {
                onShutdown -= value;
            }
        }


        internal void firePlayerConnected(UnturnedPlayer player)
        {
            OnPlayerConnected.TryInvoke(player);
        }

        public delegate void PlayerConnected(UnturnedPlayer player);
        public event PlayerConnected OnPlayerConnected;
    }
}