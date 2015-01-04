using Rocket;
using SDG;
using Steamworks;

namespace Rocket
{
    internal class RocketComponentImplementation : RocketComponent
    {

        protected override void onPlayerDisconnected(CSteamID D) { }

        protected override void onPlayerConnected(CSteamID r) { }

        protected override void onServerShutdown() { }
    }
}
