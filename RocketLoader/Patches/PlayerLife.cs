using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocket.Patches
{
    public class PlayerLife : Patch
    {
        public void Apply()
        {
            TypeDefinition t = RocketLoader.UnturnedAssembly.MainModule.GetType("SDG.PlayerLife");

            PatchHelper.UnlockByType(t, "LifeUpdated", "OnUpdateLife");
            PatchHelper.UnlockByType(t, "HealthUpdated", "OnUpdateHealth");
            PatchHelper.UnlockByType(t, "FoodUpdated", "OnUpdateFoot");
            PatchHelper.UnlockByType(t, "WaterUpdated", "OnUpdateWater");
            PatchHelper.UnlockByType(t, "VirusUpdated", "OnUpdateVirus");
            PatchHelper.UnlockByType(t, "StaminaUpdated", "OnUpdateStamina");
            PatchHelper.UnlockByType(t, "VisionUpdated", "OnUpdateVision");
            PatchHelper.UnlockByType(t, "OxygenUpdated", "OnUpdateOxygen");
            PatchHelper.UnlockByType(t, "BleedingUpdated", "OnUpdateBleeding");
            PatchHelper.UnlockByType(t, "BrokenUpdated", "OnUpdateBroken");
            PatchHelper.UnlockByType(t, "FreezingUpdated", "OnUpdateFreezing");
            PatchHelper.UnlockByType(t, "Damaged", "OnDamaged");
        }
    }
}
