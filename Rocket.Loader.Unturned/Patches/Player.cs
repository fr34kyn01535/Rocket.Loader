namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.Unturned.Player")]
    public class Player : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType("PlayerInput", "Input");
            UnlockFieldByType("PlayerStance", "Stance");
            UnlockFieldByType("PlayerEquipment", "Equipment");
            UnlockFieldByType("PlayerAnimator", "Animator");
            UnlockFieldByType("PlayerMovement", "Movement");
            UnlockFieldByType("PlayerLook", "Look");
            UnlockFieldByType("PlayerClothing", "Clothing");
            UnlockFieldByType("PlayerInventory", "Inventory");
            UnlockFieldByType("PlayerSkills", "Skills");
            UnlockFieldByType("PlayerLife", "PlayerLife");
            UnlockFieldByType("Player", "Instance");
            UnlockFieldByType("SteamChannel", "SteamChannel");
        }
    }
}