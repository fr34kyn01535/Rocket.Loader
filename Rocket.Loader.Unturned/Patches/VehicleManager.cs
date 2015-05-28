namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.VehicleManager")]
    public class VehicleManager : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType("List<InteractableVehicle>", "Vehicles");
        }
    }
}