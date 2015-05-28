namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.Palette")]
    public class Palette : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType("Color", "Server", 0);
            UnlockFieldByType("Color", "Admin", 1);
            UnlockFieldByType("Color", "Pro", 2);
            UnlockFieldByType("Color", "White", 3);
            UnlockFieldByType("Color", "Red", 4);
            UnlockFieldByType("Color", "Green", 5);
            UnlockFieldByType("Color", "Blue", 6);
            UnlockFieldByType("Color", "Orange", 7);
            UnlockFieldByType("Color", "Yellow", 8);
            UnlockFieldByType("Color", "Purple", 9);
            UnlockFieldByType("Color", "Ambient", 10);
        }
    }
}