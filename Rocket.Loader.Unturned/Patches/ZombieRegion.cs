namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.ZombieRegion")]
    public class ZombieRegion : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType("List<Zombie>", "Zombies");
        }
    }
}