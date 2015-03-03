namespace Rocket.RocketLoader.Patches
{
    public class VehicleAsset : Patch
    {
        private PatchHelper h = new PatchHelper("SDG.VehicleAsset");

        public void Apply()
        {
            h.UnlockFieldByType(typeof(string), "Name", 0);
        }
    }
}