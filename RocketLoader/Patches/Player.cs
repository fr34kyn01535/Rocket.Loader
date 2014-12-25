using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocket.Patches
{
    public class Player : Patch
    {
        public void Apply()
        {
            TypeDefinition t = RocketLoader.UnturnedAssembly.MainModule.GetType("SDG.Player");

            PatchHelper.UnlockByType(t, "sdg.playerinput", "playerInput");
            PatchHelper.UnlockByType(t, "sdg.playerstance", "playerStance");
            PatchHelper.UnlockByType(t, "sdg.playerequipment", "playerEquipment");
            PatchHelper.UnlockByType(t, "sdg.playeranimator", "playerAnimator");
            PatchHelper.UnlockByType(t, "sdg.playerhitbox", "playerHitbox");
            PatchHelper.UnlockByType(t, "sdg.playermovement", "playerMovement");
            PatchHelper.UnlockByType(t, "sdg.playerlook", "playerLook");
            PatchHelper.UnlockByType(t, "sdg.playerclothing", "playerClothing");
            PatchHelper.UnlockByType(t, "sdg.playerinventory", "playerInventory");
            PatchHelper.UnlockByType(t, "sdg.playerlife", "playerLife");
        }
    }
}
