using Mono.Cecil;

namespace Rocket.RocketLoader.Patches
{
    public class MenuPlayUI : Patch
    {
        private PatchHelper h = new PatchHelper("SDG.MenuPlayUI");

        public void Apply()
        {
            MethodDefinition ctor = h.GetMethod(".ctor");
            ctor.Body.Instructions.Clear();
        }
    }
}