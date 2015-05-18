namespace Rocket.RocketLoader.Patches
{
    public class Palette : IPatch
    {
        private PatchHelper h = new PatchHelper("SDG.Palette");

        public void Apply()
        {
            h.UnlockFieldByType("Color", "Server", 0);
            h.UnlockFieldByType("Color", "Admin", 1);
            h.UnlockFieldByType("Color", "Pro", 2);
            h.UnlockFieldByType("Color", "White", 3);
            h.UnlockFieldByType("Color", "Red", 4);
            h.UnlockFieldByType("Color", "Green", 5);
            h.UnlockFieldByType("Color", "Blue", 6);
            h.UnlockFieldByType("Color", "Orange", 7);
            h.UnlockFieldByType("Color", "Yellow", 8);
            h.UnlockFieldByType("Color", "Purple", 9);
            h.UnlockFieldByType("Color", "Ambient", 10);
        }
    }
}