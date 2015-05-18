using System;

namespace Rocket.RocketLoader.Patches
{
    public class SteamPlayer : IPatch
    {
        private PatchHelper h = new PatchHelper("SDG.SteamPlayer");

        public void Apply()
        {
            h.UnlockFieldByType("Player", "Player");
            h.UnlockFieldByType(typeof(Boolean), "IsPro", 0);
            h.UnlockFieldByType(typeof(Boolean), "IsAdmin", 1);
            h.UnlockFieldByType("SteamPlayerID", "SteamPlayerID");
            h.UnlockFieldByType("Color", "Color");
        }
    }
}