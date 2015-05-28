namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.PlayerStance")]
    public class PlayerStance : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType("EPlayerStance", "Stance");
        }
    }
}