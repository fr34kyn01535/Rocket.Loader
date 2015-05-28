namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.VehicleAsset")]
    public class VehicleAsset : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType(typeof(string), "Name");
        }
    }
}