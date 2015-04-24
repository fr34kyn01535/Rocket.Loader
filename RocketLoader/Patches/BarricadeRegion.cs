namespace Rocket.RocketLoader.Patches
{
    public class BarricadeRegion : Patch
    {
        private PatchHelper h = new PatchHelper("SDG.BarricadeRegion");

        public void Apply()
        {
            h.UnlockFieldByType("List<Transform>", "Barricades");
            h.UnlockFieldByType("List<BarricadeData>", "BarricadeDatas");
        }
    }
}