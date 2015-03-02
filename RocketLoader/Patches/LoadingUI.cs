using Mono.Cecil;

namespace Rocket.RocketLoader.Patches
{
    public class LoadingUI : Patch
    {
        private PatchHelper h = new PatchHelper("SDG.LoadingUI");

        public void Apply()
        {
            h.GetMethod(".ctor").Body.Instructions.Clear();
            h.GetMethod("Awake").Body.Instructions.Clear();
            h.GetMethod("Start").Body.Instructions.Clear();
        }
    }
}