using Rocket.RocketAPI.Commands;
using SDG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rocket.RocketAPI
{
    public class EventManager
    {
        public event Steam.ServerConnected PlayerConnected;
        public event Steam.ServerDisconnected PlayerDisconnected;

        private Dictionary<string,DateTime> players = new Dictionary<string,DateTime>();

        public EventManager()
        {
            SDG.Steam.serverConnected += onPlayerConnected;
            SDG.Steam.serverDisconnected += onPlayerDisconnected;
        }

        private void onPlayerDisconnected(Steamworks.CSteamID id)
        {
            if (PlayerDisconnected != null)
            PlayerDisconnected(id);
        }

        private void onPlayerConnected(Steamworks.CSteamID id)
        {
            if (PlayerConnected != null)
            PlayerConnected(id);
        }

        internal void Reload()
        {
            foreach (Delegate d in PlayerConnected.GetInvocationList())
            {
                PlayerConnected -= (Steam.ServerConnected)d;
            }

            foreach (Delegate d in PlayerDisconnected.GetInvocationList())
            {
                PlayerDisconnected -= (Steam.ServerDisconnected)d;
            }
        }
    }
}
