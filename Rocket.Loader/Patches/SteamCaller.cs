namespace Rocket.RocketLoader.Patches
{
    public class SteamCaller : Patch
    {
        private PatchHelper h = new PatchHelper("SDG.SteamCaller");

        public void Apply()
        {
            h.UnlockFieldByType("SteamChannel", "SteamChannel");
        }
    }
}