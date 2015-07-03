using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Linq;
namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.Unturned.BarricadeManager")]
    public class BarricadeManager : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType("BarricadeRegion[,]", "BarricadeRegions");
            UnlockFieldByType("BarricadeManager", "Instance");
        }
    }
}