namespace Rocket.RocketLoader.Patches
{
    public class PlayerSkills : Patch
    {
        private PatchHelper h = new PatchHelper("SDG.PlayerSkills");

        public void Apply()
        {
            h.UnlockFieldByType(typeof(uint), "Experience",1);
        }
    }
}