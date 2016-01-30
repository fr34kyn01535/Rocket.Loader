namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.Unturned.VehicleManager")]
    public class VehicleManager : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType("List<InteractableVehicle>", "Vehicles");
            UnlockFieldByType("VehicleManager", "Instance");
            UnlockMethodByName("sendVehicle");
        }
    }
}