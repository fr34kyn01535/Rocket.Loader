namespace Rocket.RocketLoader.Patches
{
    public class SteamPlayerID : IPatch
    {
        private PatchHelper h = new PatchHelper("SDG.SteamPlayerID");

        public void Apply()
        {
            h.UnlockFieldByType("CSteamID", "CSteamID");
            h.UnlockFieldByType("CSteamID", "SteamGroupID", 1);
            h.UnlockFieldByType(typeof(string), "SteamName", 0);
            h.UnlockFieldByType(typeof(string), "CharacterName", 1);
        }
    }
}