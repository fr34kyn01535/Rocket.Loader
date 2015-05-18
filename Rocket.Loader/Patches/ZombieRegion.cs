namespace Rocket.RocketLoader.Patches
{
    public class ZombieRegion : IPatch
    {
        private PatchHelper h = new PatchHelper("SDG.ZombieRegion");

        public void Apply()
        {
            h.UnlockFieldByType("List<Zombie>", "Zombies");
        }
    }
}