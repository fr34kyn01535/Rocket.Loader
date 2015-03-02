using Mono.Cecil;

namespace Rocket.RocketLoader.Patches
{
    public class MenuUI : Patch
    {
        private PatchHelper h = new PatchHelper("SDG.MenuUI");

        public void Apply()
        {
            h.GetMethod(".ctor").Body.Instructions.Clear();
            h.GetMethod("Awake").Body.Instructions.Clear();
            h.GetMethod("Start").Body.Instructions.Clear();
        }
    }
}