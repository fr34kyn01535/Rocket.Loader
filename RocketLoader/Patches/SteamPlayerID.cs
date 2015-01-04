﻿using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Rocket.Patches
{
    public class SteamPlayerID : Patch
    {
        public void Apply()
        {
            TypeDefinition t = RocketLoader.UnturnedAssembly.MainModule.GetType("SDG.SteamPlayerID");
            PatchHelper.UnlockByType(t, "CSteamID", "CSteamId", 0);

            PatchHelper.UnlockByType(t, "string", "SteamName", 0);
            PatchHelper.UnlockByType(t, "string", "IngameName", 0);
        }
    }
}
