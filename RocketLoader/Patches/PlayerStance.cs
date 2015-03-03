namespace Rocket.RocketLoader.Patches
{
    public class PlayerStance : Patch
    {
        private PatchHelper h = new PatchHelper("SDG.PlayerStance");

        public void Apply()
        {
            h.UnlockFieldByType("EPlayerStance", "Stance");
        }
    }
}