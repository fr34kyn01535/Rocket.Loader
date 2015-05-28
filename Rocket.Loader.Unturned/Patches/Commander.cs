namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.Commander")]
    public class Commander : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType("List<Command>", "Commands", 0);
        }
    }
}