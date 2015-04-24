namespace Rocket.RocketLoader.Patches
{
    public class VehicleManager : Patch
    {
        private PatchHelper h = new PatchHelper("SDG.VehicleManager");

        public void Apply()
        {
            h.UnlockFieldByType("List<InteractableVehicle>", "Vehicles");
        }
    }
}