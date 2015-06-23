namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.Unturned.VehicleAsset")]
    public class VehicleAsset : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType(typeof(string), "Name");
        }
    }
}