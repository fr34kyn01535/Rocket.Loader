namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.BarricadeManager")]
    public class BarricadeManager : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType("BarricadeRegion[,]", "BarricadeRegions");
        }
    }
}