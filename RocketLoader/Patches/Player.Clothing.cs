namespace Rocket.RocketLoader.Patches
{
    public class PlayerClothing : Patch
    {
        private PatchHelper h = new PatchHelper("SDG.PlayerClothing");

        public void Apply()
        {
            h.UnlockFieldByType("ShirtUpdated", "OnShirtUpdated");
            h.UnlockFieldByType("PantsUpdated", "OnPantsUpdated");
            h.UnlockFieldByType("BackpackUpdated", "OnBackpackUpdated");
            h.UnlockFieldByType("VestUpdated", "OnVestUpdated");
            h.UnlockFieldByType("MaskUpdated", "OnMaskUpdated");
            h.UnlockFieldByType("HatUpdated", "OnHatUpdated");
            h.UnlockFieldByType("GlassesUpdated", "OnGlassesUpdated");
        }
    }
}