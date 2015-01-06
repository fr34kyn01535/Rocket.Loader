using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocket.RocketLoader.Patches
{
    public class Commander : Patch
    {
        PatchHelper h = new PatchHelper("SDG.Commander");

        public void Apply()
        {
            h.UnlockFieldByType("Command[]", "Commands");
        }
    }
}
