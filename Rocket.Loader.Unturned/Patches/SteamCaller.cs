namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.SteamCaller")]
    public class SteamCaller : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType("SteamChannel", "SteamChannel");
        }
    }
}