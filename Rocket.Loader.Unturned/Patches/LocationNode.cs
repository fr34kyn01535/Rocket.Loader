using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.Unturned.LocationNode")]
    public class LocationNode : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType("string", "Name");
        }
    }
}