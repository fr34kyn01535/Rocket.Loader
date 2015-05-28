namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.StructureManager")]
    public class StructureManager : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType("StructureRegion[,]", "StructureRegions");
            UnlockFieldByType("StructureManager", "Instance");
        }
    }
}