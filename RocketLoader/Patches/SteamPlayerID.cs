using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Rocket.RocketLoader.Patches
{
    public class SteamPlayerID : Patch
    {
        PatchHelper h = new PatchHelper("SDG.SteamPlayerID");

        public void Apply()
        {
            h.UnlockFieldByType("CSteamID", "CSteamID", 0);
            h.UnlockFieldByType(typeof(string), "SteamName", 0);
            h.UnlockFieldByType(typeof(string), "CharacterName", 1);
        }
    }
}
