namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.Unturned.PlayerSkills")]
    public class PlayerSkills : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType(typeof(uint), "Experience",1);
        }
    }
}