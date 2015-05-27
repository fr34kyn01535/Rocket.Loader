namespace Rocket.RocketLoader.Patches
{
    public class StructureManager : IPatch
    {
        private PatchHelper h = new PatchHelper("SDG.StructureManager");

        public void Apply()
        {
            h.UnlockFieldByType("StructureRegion[,]", "StructureRegions");
            h.UnlockFieldByType("StructureManager", "Instance");
        }
    }
}