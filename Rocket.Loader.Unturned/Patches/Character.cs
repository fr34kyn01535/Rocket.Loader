namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.Character")]
    public class Character : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType("CSteamID", "CSteamID");
        }
    }
}