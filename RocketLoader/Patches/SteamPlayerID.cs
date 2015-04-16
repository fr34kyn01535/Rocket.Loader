namespace Rocket.RocketLoader.Patches
{
    public class SteamPlayerID : Patch
    {
        private PatchHelper h = new PatchHelper("SDG.SteamPlayerID");

        public void Apply()
        {
            h.UnlockFieldByType("CSteamID", "CSteamID", 0);
            h.UnlockFieldByType("CSteamID", "SteamGroupID", 1);
            h.UnlockFieldByType(typeof(string), "SteamName", 0);
            h.UnlockFieldByType(typeof(string), "CharacterName", 1);
        }
    }
}