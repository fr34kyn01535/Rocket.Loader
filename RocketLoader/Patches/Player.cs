namespace Rocket.RocketLoader.Patches
{
    public class Player : Patch
    {
        private PatchHelper h = new PatchHelper("SDG.Player");

        public void Apply()
        {
            h.UnlockFieldByType("PlayerInput", "Input");
            h.UnlockFieldByType("PlayerStance", "Stance");
            h.UnlockFieldByType("PlayerEquipment", "Equipment");
            h.UnlockFieldByType("PlayerAnimator", "Animator");
            h.UnlockFieldByType("PlayerHitbox", "Hitbox");
            h.UnlockFieldByType("PlayerMovement", "Movement");
            h.UnlockFieldByType("PlayerLook", "Look");
            h.UnlockFieldByType("PlayerClothing", "Clothing");
            h.UnlockFieldByType("PlayerInventory", "Inventory");

            h.UnlockFieldByType("PlayerLife", "PlayerLife");
            h.UnlockFieldByType("Player", "Instance");
            h.UnlockFieldByType("SteamChannel", "SteamChannel");
        }
    }
}