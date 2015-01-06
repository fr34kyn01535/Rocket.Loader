using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocket.RocketLoader.Patches
{
    public class Command : Patch
    {
        PatchHelper h = new PatchHelper("SDG.Command");

        public void Apply()
        {
            h.UnlockFieldByType(typeof(string), "commandName", 0);
            h.UnlockFieldByType(typeof(string), "commandInfo", 1);
            h.UnlockFieldByType(typeof(string), "commandHelp", 2);
            h.UnlockFieldByType("Local", "Local");
        }
    }
}
