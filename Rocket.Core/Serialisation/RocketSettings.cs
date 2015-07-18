using Rocket.API;
using Rocket.Core.Assets;
using Rocket.Core.Logging;
using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace Rocket.Core.Serialization
{
    public sealed class RemoteConsole
    {
        public bool Enabled = false;
        public short Port = 27115;
        public string Password = "changeme";
    }

    public sealed class AutomaticShutdown
    {
        public bool Enabled = false;
        public int Interval = 86400;
    }

    public sealed class WebPermissions
    {
        public bool Enabled = false;
        public string Url = "";
        public int Interval = 180;
    }

    public sealed class WebConfigurations
    {
        public bool Enabled = false;
        public string Url = "";
    }

    public sealed class RocketSettings{
        [XmlElement("RCON")]
        public RemoteConsole RCON = new RemoteConsole();

        [XmlElement("AutomaticShutdown")]
        public AutomaticShutdown AutomaticShutdown = new AutomaticShutdown();

        public WebConfigurations WebConfigurations = new WebConfigurations();
        public WebPermissions WebPermissions = new WebPermissions();

        public string LanguageCode = "en";
    }
}