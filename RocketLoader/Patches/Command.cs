using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocket.Patches
{
    public class Command : Patch
    {
        public void Apply()
        {
            TypeDefinition t = RocketLoader.UnturnedAssembly.MainModule.GetType("SDG.Command");

            PatchHelper.UnlockByType(t, "system.string", new string[] { "commandName", "commandInfo", "commandHelp" });
        }
    }
}
