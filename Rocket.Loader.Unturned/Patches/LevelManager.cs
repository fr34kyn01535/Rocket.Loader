using System.Collections.Generic;
namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.Unturned.LevelManager")]
    public class LevelManager : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType("EArenaState", "ArenaState");
            UnlockFieldByType("EArenaMessage", "ArenaMessage");
            UnlockFieldByType("List<CSteamID>", "ArenaGroups");
            UnlockFieldByType("List<ArenaPlayer>", "ArenaPlayers");
        }
    }
}