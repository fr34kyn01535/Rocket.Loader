namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.Unturned.PlayerStance")]
    public class PlayerStance : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType("EPlayerStance", "Stance");
        }
    }
}