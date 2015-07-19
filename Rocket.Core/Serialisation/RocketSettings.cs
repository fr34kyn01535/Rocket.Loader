using System.Xml.Serialization;

namespace Rocket.Core.Serialization
{
    public sealed class RemoteConsole
    {
        [XmlAttribute]
        public bool Enabled = false;
        [XmlAttribute]
        public short Port = 27115;
        [XmlAttribute]
        public string Password = "changeme";
    }

    public sealed class AutomaticShutdown
    {
        [XmlAttribute]
        public bool Enabled = false;
        [XmlAttribute]
        public int Interval = 86400;
    }

    public sealed class WebPermissions
    {
        [XmlAttribute]
        public bool Enabled = false;
        [XmlAttribute]
        public string Url = "";
        [XmlAttribute]
        public int Interval = 180;
    }

    public sealed class WebConfigurations
    {
        [XmlAttribute]
        public bool Enabled = false;
        [XmlAttribute]  
        public string Url = "";
    }

    public sealed class RocketSettings{
        [XmlElement("RCON")]
        public RemoteConsole RCON = new RemoteConsole();

        [XmlElement("AutomaticShutdown")]
        public AutomaticShutdown AutomaticShutdown = new AutomaticShutdown();

        [XmlElement("WebConfigurations")]
        public WebConfigurations WebConfigurations = new WebConfigurations();

        [XmlElement("WebPermissions")]
        public WebPermissions WebPermissions = new WebPermissions();

        [XmlElement("LanguageCode")]
        public string LanguageCode = "en";
    }
}