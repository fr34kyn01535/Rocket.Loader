namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.Unturned.ZombieManager")]
    public class ZombieManager : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType("ZombieRegion[]", "ZombieRegions");
        }
    }
}