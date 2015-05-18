namespace Rocket.RocketLoader.Patches
{
    public class Commander : IPatch
    {
        private PatchHelper h = new PatchHelper("SDG.Commander");

        public void Apply()
        {
            h.UnlockFieldByType("Command[]", "Commands");
        }
    }
}