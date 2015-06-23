namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.Unturned.SteamPlayerID")]
    public class SteamPlayerID : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType("CSteamID", "CSteamID");
            UnlockFieldByType("CSteamID", "SteamGroupID", 1);
            UnlockFieldByType(typeof(string), "SteamName", 0);
            UnlockFieldByType(typeof(string), "CharacterName", 1);
        }
    }
}