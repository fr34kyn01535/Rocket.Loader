using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Linq;

namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.Unturned.Dedicator")]
    public class Dedicator : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType(typeof(string), "InstanceName");
        }
    }
}