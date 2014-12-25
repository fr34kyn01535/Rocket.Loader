using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Rocket.Patches
{
    public class Steam : Patch
    {
        public void Apply()
        {
            TypeDefinition t = RocketLoader.UnturnedAssembly.MainModule.GetType("SDG.Steam");

            FieldDefinition[] stringFields = t.Fields.Where(f => f.FieldType.FullName.ToLower() == "system.string").ToArray();

            PatchHelper.UnlockByType(t, "sdg.steam/clientconnected", "clientConnected");
            PatchHelper.UnlockByType(t, "sdg.steam/clientdisconnected", "clientDisconnected");
            PatchHelper.UnlockByType(t, "sdg.steam/serverhosted", "serverHosted");
            PatchHelper.UnlockByType(t, "sdg.steam/servershutdown", "serverShutdown");
            PatchHelper.UnlockByType(t, "sdg.steam/serverconnected", "serverConnected");
            PatchHelper.UnlockByType(t, "sdg.steam/serverdisconnected", "serverDisconnected");
        }
    }
}
