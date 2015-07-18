namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.Unturned.LocationNode")]
    public class LocationNode : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType("string", "Name");
        }
    }
}