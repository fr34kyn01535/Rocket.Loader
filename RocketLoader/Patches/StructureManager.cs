namespace Rocket.RocketLoader.Patches
{
    public class StructureManager : Patch
    {
        private PatchHelper h = new PatchHelper("SDG.StructureManager");

        public void Apply()
        {
            h.UnlockFieldByType("List<Transform>", "Structures");
            h.UnlockFieldByType("List<StructureData>", "StructureDatas");
        }
    }
}