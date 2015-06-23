namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.Unturned.PlayerLife")]
    public class PlayerLife : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType("LifeUpdated", "OnUpdateLife");
            UnlockFieldByType("HealthUpdated", "OnUpdateHealth");
            UnlockFieldByType("FoodUpdated", "OnUpdateFood");
            UnlockFieldByType("WaterUpdated", "OnUpdateWater");
            UnlockFieldByType("VirusUpdated", "OnUpdateVirus");
            UnlockFieldByType("StaminaUpdated", "OnUpdateStamina");
            UnlockFieldByType("VisionUpdated", "OnUpdateVision");
            UnlockFieldByType("OxygenUpdated", "OnUpdateOxygen");
            UnlockFieldByType("BleedingUpdated", "OnUpdateBleeding");
            UnlockFieldByType("BrokenUpdated", "OnUpdateBroken");
            UnlockFieldByType("FreezingUpdated", "OnUpdateFreezing");
            UnlockFieldByType("Damaged", "OnDamaged");
            UnlockFieldByType("CSteamID", "CSteamID");

            UnlockFieldByType(typeof(byte), "Health", 1);
            UnlockFieldByType(typeof(byte), "Hunger", 3);
            UnlockFieldByType(typeof(byte), "Thirst", 4);
            UnlockFieldByType(typeof(byte), "Infection", 5);
            UnlockFieldByType(typeof(byte), "Life", 6);
            UnlockFieldByType(typeof(byte), "Stamina", 7);
            UnlockFieldByType(typeof(byte), "Breath", 8);

            UnlockFieldByType(typeof(bool), "Dead", 0);
            UnlockFieldByType(typeof(bool), "Bleeding", 1);
            UnlockFieldByType(typeof(bool), "Broken", 2);
            UnlockFieldByType(typeof(bool), "Freezing", 3);
        }
    }
}