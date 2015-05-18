using System.Collections.Generic;
namespace Rocket.RocketLoader.Patches
{
    public class LevelZombies : IPatch
    {
        private PatchHelper h = new PatchHelper("SDG.LevelZombies");

        public void Apply()
        {
            h.UnlockFieldByType("List<ZombieSpawnpoint>[,]", "ZombieSpawnPositions");
            h.UnlockFieldByType("List<ZombieSpawnpoint>[]", "ZombieSpawns");
            h.UnlockFieldByType("List<ZombieTable>", "ZombieTypes");
        }
    }
}