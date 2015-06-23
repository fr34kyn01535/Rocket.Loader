namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.Unturned.PlayerEquipment")]
    public class PlayerEquipment : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType(typeof(ushort), "HoldingItemID");
        }
    }
}