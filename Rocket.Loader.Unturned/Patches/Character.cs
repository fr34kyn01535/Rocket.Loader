namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.Unturned.Character")]
    public class Character : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType("CSteamID", "CSteamID");
        }
    }
}