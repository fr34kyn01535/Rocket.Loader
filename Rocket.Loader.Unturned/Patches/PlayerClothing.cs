namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.Unturned.PlayerClothing")]
    public class PlayerClothing : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType("ShirtUpdated", "OnShirtUpdated");
            UnlockFieldByType("PantsUpdated", "OnPantsUpdated");
            UnlockFieldByType("BackpackUpdated", "OnBackpackUpdated");
            UnlockFieldByType("VestUpdated", "OnVestUpdated");
            UnlockFieldByType("MaskUpdated", "OnMaskUpdated");
            UnlockFieldByType("HatUpdated", "OnHatUpdated");
            UnlockFieldByType("GlassesUpdated", "OnGlassesUpdated");
        }
    }
}