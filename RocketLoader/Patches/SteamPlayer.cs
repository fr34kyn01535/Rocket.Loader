using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Rocket.Patches
{
    public class SteamPlayer : Patch
    {
        public void Apply()
        {
            TypeDefinition t = RocketLoader.UnturnedAssembly.MainModule.GetType("SDG.SteamPlayer");
            PatchHelper.UnlockByType(t, "SDG.Player", "Player", 0);
        }
    }
}
