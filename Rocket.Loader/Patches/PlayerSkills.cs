namespace Rocket.RocketLoader.Patches
{
    public class PlayerSkills : IPatch
    {
        private PatchHelper h = new PatchHelper("SDG.PlayerSkills");

        public void Apply()
        {
            h.UnlockFieldByType(typeof(uint), "Experience",1);
        }
    }
}