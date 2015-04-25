namespace Rocket.RocketLoader.Patches
{
    public class ZombieRegion : Patch
    {
        private PatchHelper h = new PatchHelper("SDG.ZombieRegion");

        public void Apply()
        {
            h.UnlockFieldByType("List<Zombie>", "Zombies");
        }
    }
}