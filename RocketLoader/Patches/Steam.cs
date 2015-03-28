﻿using Mono.Cecil;
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
            h.UnlockFieldByType(typeof(string), "InstanceName", 13);
            h.UnlockFieldByType(typeof(uint), "ServerPort", 1);
            h.UnlockFieldByType(typeof(byte), "MaxPlayers");

            h.UnlockFieldByType(typeof(bool), "PvP",5);

            h.UnlockFieldByType("List<SteamPlayer>", "Players");

            MethodDefinition reject = h.Type.Methods.AsEnumerable().Where(m => m.Parameters.Count == 2 &&
                 m.Parameters[0].ParameterType.Name == "CSteamID" &&
                 m.Parameters[1].ParameterType.Name == "ESteamRejection").FirstOrDefault();
            reject.Name = "Reject";
            reject.IsPublic = true;

        }
    }
}