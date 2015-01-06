using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocket.RocketLoader.Patches
{
    public class PlayerLife : Patch
    {
        PatchHelper h = new PatchHelper("SDG.PlayerLife");

        public void Apply()
        {
            h.UnlockFieldByType("LifeUpdated", "OnUpdateLife");
            h.UnlockFieldByType("HealthUpdated", "OnUpdateHealth");
            h.UnlockFieldByType("FoodUpdated", "OnUpdateFoot");
            h.UnlockFieldByType("WaterUpdated", "OnUpdateWater");
            h.UnlockFieldByType("VirusUpdated", "OnUpdateVirus");
            h.UnlockFieldByType("StaminaUpdated", "OnUpdateStamina");
            h.UnlockFieldByType("VisionUpdated", "OnUpdateVision");
            h.UnlockFieldByType("OxygenUpdated", "OnUpdateOxygen");
            h.UnlockFieldByType("BleedingUpdated", "OnUpdateBleeding");
            h.UnlockFieldByType("BrokenUpdated", "OnUpdateBroken");
            h.UnlockFieldByType("FreezingUpdated", "OnUpdateFreezing");
            h.UnlockFieldByType("Damaged", "OnDamaged");
            h.UnlockFieldByType("CSteamID", "CSteamID");

            h.UnlockFieldByType(typeof(byte), "Health", 1);
            //h.UnlockFieldByType(typeof(byte), "??", 1); //v
            h.UnlockFieldByType(typeof(byte), "Hunger", 3);
            h.UnlockFieldByType(typeof(byte), "Thirst", 4); 
            h.UnlockFieldByType(typeof(byte), "Infection", 5);
            h.UnlockFieldByType(typeof(byte), "Life", 6); 
            h.UnlockFieldByType(typeof(byte), "Stamina", 7);
            h.UnlockFieldByType(typeof(byte), "Breath", 8); 

            h.UnlockFieldByType(typeof(bool), "Dead", 0);
            h.UnlockFieldByType(typeof(bool), "Bleeding", 1);
            h.UnlockFieldByType(typeof(bool), "Broken", 2);
            h.UnlockFieldByType(typeof(bool), "Freezing", 3);
        }
    }
}
