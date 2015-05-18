namespace Rocket.RocketLoader.Patches
{
    public class PlayerEquipment : IPatch
    {
        private PatchHelper h = new PatchHelper("SDG.PlayerEquipment");

        public void Apply()
        {
            h.UnlockFieldByType(typeof(ushort), "HoldingItemID");
        }
    }
}