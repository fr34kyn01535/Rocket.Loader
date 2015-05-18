namespace Rocket.RocketLoader.Patches
{
    public class VehicleManager : IPatch
    {
        private PatchHelper h = new PatchHelper("SDG.VehicleManager");

        public void Apply()
        {
            h.UnlockFieldByType("List<InteractableVehicle>", "Vehicles");
        }
    }
}