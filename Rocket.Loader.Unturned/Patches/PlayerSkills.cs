namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.PlayerSkills")]
    public class PlayerSkills : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType(typeof(uint), "Experience",1);
        }
    }
}