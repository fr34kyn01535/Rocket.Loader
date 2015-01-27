using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocket.RocketLoader.Patches
{
    public class Steam : Patch
    {
        PatchHelper h = new PatchHelper("SDG.Steam");

        public void Apply()
        {
            h.UnlockFieldByType("ClientConnected", "OnClientConnected");
            h.UnlockFieldByType("ClientDisconnected", "OnClientDisconnected");
            h.UnlockFieldByType("ServerHosted", "OnServerHosted");
            h.UnlockFieldByType("ServerShutdown", "OnServerShutdown");
            h.UnlockFieldByType("ServerConnected", "OnServerConnected");
            h.UnlockFieldByType("ServerDisconnected", "OnServerDisconnected");
            h.UnlockFieldByType(typeof(string), "InstanceName", 7);
            h.UnlockFieldByType(typeof(uint), "ServerPort", 1);

            h.UnlockFieldByType("List<SDG.SteamPlayer>", "Players");
        }
    }
}
