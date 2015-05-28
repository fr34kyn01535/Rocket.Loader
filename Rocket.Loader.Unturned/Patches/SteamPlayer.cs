using System;

namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.SteamPlayer")]
    public class SteamPlayer : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType("Player", "Player");
            UnlockFieldByType(typeof(Boolean), "IsPro", 0);
            UnlockFieldByType(typeof(Boolean), "IsAdmin", 1);
            UnlockFieldByType("SteamPlayerID", "SteamPlayerID");
            UnlockFieldByType("Color", "Color");
        }
    }
}