namespace Rocket.RocketLoader.Patches
{
    public class StructureRegion : IPatch
    {
        private PatchHelper h = new PatchHelper("SDG.StructureRegion");

        public void Apply()
        {
            h.UnlockFieldByType("List<Transform>", "Structures");
            h.UnlockFieldByType("List<StructureData>", "StructureDatas");
        }
    }
}