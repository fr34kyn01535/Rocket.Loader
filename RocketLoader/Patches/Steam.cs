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

            PatchHelper.UnlockByType(t, "clientconnected", "clientConnected");
            PatchHelper.UnlockByType(t, "clientdisconnected", "clientDisconnected");
            PatchHelper.UnlockByType(t, "serverhosted", "serverHosted");
            PatchHelper.UnlockByType(t, "servershutdown", "serverShutdown");
            PatchHelper.UnlockByType(t, "serverconnected", "serverConnected");
            PatchHelper.UnlockByType(t, "serverdisconnected", "serverDisconnected");

            PatchHelper.UnlockByType(t, "string", "Servername", 7);
        }
    }
}
