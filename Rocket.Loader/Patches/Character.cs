namespace Rocket.RocketLoader.Patches
{
    public class Character : IPatch
    {
        private PatchHelper h = new PatchHelper("SDG.Character");

        public void Apply()
        {
            h.UnlockFieldByType("CSteamID", "CSteamID");
        }
    }
}