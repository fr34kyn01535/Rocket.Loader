namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.Unturned.SteamCaller")]
    public class SteamCaller : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType("SteamChannel", "SteamChannel");
        }
    }
}