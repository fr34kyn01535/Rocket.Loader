namespace Rocket.RocketLoader.Patches
{
    public class StructureManager : IPatch
    {
        private PatchHelper h = new PatchHelper("SDG.StructureManager");

        public void Apply()
        {
            h.UnlockFieldByType("List<Transform>", "Structures");
            h.UnlockFieldByType("List<StructureData>", "StructureDatas");
            h.UnlockFieldByType("StructureManager", "Instance");
        }
    }
}