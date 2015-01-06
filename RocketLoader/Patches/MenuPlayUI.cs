using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocket.RocketLoader.Patches
{
    public class MenuPlayUI : Patch
    {
        PatchHelper h = new PatchHelper("SDG.MenuPlayUI");

        public void Apply()
        {
            MethodDefinition ctor = h.GetMethod(".ctor");
            ctor.Body.Instructions.Clear();
        }
    }
}
