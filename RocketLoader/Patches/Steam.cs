using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Linq;

namespace Rocket.RocketLoader.Patches
{
    public class Steam : Patch
    {
        private PatchHelper h = new PatchHelper("SDG.Steam");

        public void Apply()
        {
            h.UnlockFieldByType("ClientConnected", "OnClientConnected");
            h.UnlockFieldByType("ClientDisconnected", "OnClientDisconnected");
            h.UnlockFieldByType("ServerHosted", "OnServerHosted");
            h.UnlockFieldByType("ServerShutdown", "OnServerShutdown");
            h.UnlockFieldByType("ServerConnected", "OnServerConnected");
            h.UnlockFieldByType("ServerDisconnected", "OnServerDisconnected");
            h.UnlockFieldByType(typeof(string), "Version");
            h.UnlockFieldByType(typeof(string), "InstanceName", 10);
            h.UnlockFieldByType(typeof(uint), "ServerPort", 1);
            h.UnlockFieldByType(typeof(byte), "MaxPlayers");

            h.UnlockFieldByType("List<SDG.SteamPlayer>", "Players");
        }
    }
}