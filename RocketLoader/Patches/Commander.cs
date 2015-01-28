namespace Rocket.RocketLoader.Patches
{
    public class Commander : Patch
    {
        private PatchHelper h = new PatchHelper("SDG.Commander");

        public void Apply()
        {
            h.UnlockFieldByType("Command[]", "Commands");
        }
    }
}