﻿using System;

namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.EffectManager")]
    public class EffectManager : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType("EffectManager", "Instance");
        }
    }
}