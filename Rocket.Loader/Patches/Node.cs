﻿using System.Collections.Generic;
namespace Rocket.RocketLoader.Patches
{
    public class Node : IPatch
    {
        private PatchHelper h = new PatchHelper("SDG.Node");

        public void Apply()
        {
            h.UnlockFieldByType("Vector3", "Position");
            h.UnlockFieldByType("ENodeType", "NodeType");
        }
    }
}