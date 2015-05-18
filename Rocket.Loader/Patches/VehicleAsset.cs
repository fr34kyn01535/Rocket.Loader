namespace Rocket.RocketLoader.Patches
{
    public class VehicleAsset : IPatch
    {
        private PatchHelper h = new PatchHelper("SDG.VehicleAsset");

        public void Apply()
        {
            h.UnlockFieldByType(typeof(string), "Name");
        }
    }
}