namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.Unturned.Command")]
    public class Command : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType(typeof(string), "commandName", 0);
            UnlockFieldByType(typeof(string), "commandInfo", 1);
            UnlockFieldByType(typeof(string), "commandHelp", 2);
            UnlockFieldByType("Local", "Local");
        }
    }
}