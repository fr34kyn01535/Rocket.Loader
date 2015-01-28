namespace Rocket.RocketLoader.Patches
{
    public class Character : Patch
    {
        private PatchHelper h = new PatchHelper("SDG.Character");

        public void Apply()
        {
            h.UnlockFieldByType("CSteamID", "CSteamID");
        }
    }
}