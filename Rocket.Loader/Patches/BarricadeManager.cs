namespace Rocket.RocketLoader.Patches
{
    public class BarricadeManager : IPatch
    {
        private PatchHelper h = new PatchHelper("SDG.BarricadeManager");

        public void Apply()
        {
            h.UnlockFieldByType("BarricadeRegion[,]", "BarricadeRegions");
        }
    }
}