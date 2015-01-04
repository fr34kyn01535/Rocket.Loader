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

            PatchHelper.UnlockByType(t, "playerinput", "playerInput");
            PatchHelper.UnlockByType(t, "playerstance", "playerStance");
            PatchHelper.UnlockByType(t, "playerequipment", "playerEquipment");
            PatchHelper.UnlockByType(t, "playeranimator", "playerAnimator");
            PatchHelper.UnlockByType(t, "playerhitbox", "playerHitbox");
            PatchHelper.UnlockByType(t, "playermovement", "playerMovement");
            PatchHelper.UnlockByType(t, "playerlook", "playerLook");
            PatchHelper.UnlockByType(t, "playerclothing", "playerClothing");
            PatchHelper.UnlockByType(t, "playerinventory", "playerInventory");

            PatchHelper.UnlockByType(t, "PlayerLife", "PlayerLife");
            PatchHelper.UnlockByType(t, "Player", "Instance");

        }
    }
}
