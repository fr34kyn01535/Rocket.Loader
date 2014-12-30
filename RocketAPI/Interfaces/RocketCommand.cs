using SDG;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Rocket.RocketAPI
{
    public interface RocketCommand
    {
        void Execute(SteamPlayerID caller, string command);

        string Name { get; }

        string Help { get; }
    }
}
