using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace RocketLauncher
{
    public enum VersionType { Beta, Release };
    public class Build
    {
        public VersionType Type;
        public string Url;
        public string Version;
    }

}
