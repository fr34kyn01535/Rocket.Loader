namespace Rocket.RocketLoader.Patches
{
    public class ZombieManager : Patch
    {
        private PatchHelper h = new PatchHelper("SDG.ZombieManager");

        public void Apply()
        {
            h.UnlockFieldByType("ZombieRegion[]", "ZombieRegions");
        }
    }
}