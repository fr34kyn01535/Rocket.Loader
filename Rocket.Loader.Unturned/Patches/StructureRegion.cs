﻿namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.Unturned.StructureRegion")]
    public class StructureRegion : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType("List<Transform>", "Structures");
            UnlockFieldByType("List<StructureData>", "StructureDatas");
        }
    }
}