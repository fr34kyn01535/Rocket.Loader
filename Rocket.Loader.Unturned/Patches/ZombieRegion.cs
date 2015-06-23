namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.Unturned.ZombieRegion")]
    public class ZombieRegion : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType("List<Zombie>", "Zombies");
        }
    }
}