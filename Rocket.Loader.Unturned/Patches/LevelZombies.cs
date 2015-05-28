using System.Collections.Generic;
namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.LevelZombies")]
    public class LevelZombies : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType("List<ZombieSpawnpoint>[,]", "ZombieSpawnPositions");
            UnlockFieldByType("List<ZombieSpawnpoint>[]", "ZombieSpawns");
            UnlockFieldByType("List<ZombieTable>", "ZombieTypes");
        }
    }
}