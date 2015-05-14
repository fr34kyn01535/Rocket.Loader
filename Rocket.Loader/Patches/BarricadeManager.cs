namespace Rocket.RocketLoader.Patches
{
    public class BarricadeManager : Patch
    {
        private PatchHelper h = new PatchHelper("SDG.BarricadeManager");

        public void Apply()
        {
            h.UnlockFieldByType("BarricadeRegion[,]", "BarricadeRegions");
        }
    }
}