using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace Rocket.RocketAPI
{
    public interface RocketPlugin
    {
        string Author { get; }

        void Load();

    }
}
