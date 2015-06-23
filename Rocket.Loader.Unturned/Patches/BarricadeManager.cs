namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.Unturned.BarricadeManager")]
    public class BarricadeManager : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType("BarricadeRegion[,]", "BarricadeRegions");
        }
    }
}