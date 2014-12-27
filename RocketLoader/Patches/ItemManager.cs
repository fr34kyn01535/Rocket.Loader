using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Rocket.Patches
{
    public class ItemManager : Patch
    {
        public void Apply()
        {
            TypeDefinition t = RocketLoader.UnturnedAssembly.MainModule.GetType("SDG.ItemManager");

            foreach (MethodDefinition method in t.Methods)
            {
                PatchHelper.Unlock(method);
            }
        }
    }
}
