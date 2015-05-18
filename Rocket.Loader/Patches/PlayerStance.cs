namespace Rocket.RocketLoader.Patches
{
    public class PlayerStance : IPatch
    {
        private PatchHelper h = new PatchHelper("SDG.PlayerStance");

        public void Apply()
        {
            h.UnlockFieldByType("EPlayerStance", "Stance");
        }
    }
}