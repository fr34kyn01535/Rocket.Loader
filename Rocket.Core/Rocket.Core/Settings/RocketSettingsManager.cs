using Rocket.API;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace Rocket.Core.Settings
{
    public sealed class RCONSettingsSection {

        private string password = "changeme";
        private int port = 0;
        private bool enabled = false;
        private bool minimal = false;

        [XmlAttribute(AttributeName = "Enabled")]
        public bool Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                enabled = value;
            }
        }

        [XmlAttribute(AttributeName = "Minimal")]
        public bool Minimal
        {
            get
            {
                return minimal;
            }
            set
            {
                minimal = value;
            }
        }


        [XmlAttribute(AttributeName = "Password")]
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
            }
        }

        [XmlAttribute(AttributeName = "Port")]
        public int Port
        {
            get
            {
                return port;
            }
            set
            {
                port = value; ;
            }
        }
    }

    public sealed class AutomaticShutdownSettingsSection
    {
        private int interval = 0;
        private bool enabled = false;

        [XmlAttribute(AttributeName = "Enabled")]
        public bool Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                enabled = value;
            }
        }

        [XmlAttribute(AttributeName = "Interval")]
        public int Interval
        {
            get
            {
                return interval;
            }
            set
            {
                interval = value;
            }
        }
    }

    public sealed class WebPermissionsSettingsSection
    {
        private bool enabled = false;
        private string url = "";
        private int interval = 180;

        [XmlAttribute(AttributeName = "Enabled")]
        public bool Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                enabled = value;
            }
        }

        [XmlAttribute(AttributeName = "Url")]
        public string Url
        {
            get
            {
                return url;
            }
            set
            {
                url = value;
            }
        }

        [XmlAttribute(AttributeName = "Interval")]
        public int Interval
        {
            get
            {
                return interval;
            }
            set
            {
                interval = value;
            }
        }

    }

    public sealed class WebConfigurationsSettingsSection
    {
        private bool enabled = false;
        private string url = "";

        [XmlAttribute(AttributeName = "Enabled")]
        public bool Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                enabled = value;
            }
        }

        [XmlAttribute(AttributeName = "Url")]
        public string Url
        {
            get
            {
                return url;
            }
            set
            {
                url = value;
            }
        }
    }

    public sealed class Settings{
        
        private RCONSettingsSection rcon = new RCONSettingsSection();
        private AutomaticShutdownSettingsSection automaticShutdown = new AutomaticShutdownSettingsSection();

        private WebConfigurationsSettingsSection webConfigurations = new WebConfigurationsSettingsSection();
        private WebPermissionsSettingsSection webPermissions = new WebPermissionsSettingsSection();

        private bool enableJoinLeaveMessages = false;
        private string languageCode = "en";
        private int automaticSaveInterval = 300;

        private IRocketImplementationConfigurationSection implementatonConfiguration = RocketBootstrap.Implementation.Configuration;

        [XmlElement(ElementName = "AutomaticShutdown")]
        public AutomaticShutdownSettingsSection AutomaticShutdown
        {
            get
            {
                return automaticShutdown;
            }
            set
            {
                automaticShutdown = value;
            }
        }

        [XmlElement(ElementName = "LanguageCode")]
        public string LanguageCode
        {
            get
            {
                return languageCode;
            }
            set
            {
                languageCode = value;
            }
        }

        [XmlElement(ElementName = "EnableJoinLeaveMessages")]
        public bool EnableJoinLeaveMessages
        {
            get
            {
                return enableJoinLeaveMessages;
            }
            set
            {
                enableJoinLeaveMessages = value;
            }
        }

        [XmlElement(ElementName = "AutomaticSaveInterval")]
        public int AutomaticSaveInterval
        {
            get
            {
                return automaticSaveInterval;
            }
            set
            {
                automaticSaveInterval = value; ;
            }
        }

        [XmlElement(ElementName = "WebPermissions")]
        public WebPermissionsSettingsSection WebPermissions
        {
            get
            {
                return webPermissions;
            }
            set
            {
                webPermissions = value;
            }
        }

        [XmlElement(ElementName = "WebConfigurations")]
        public WebConfigurationsSettingsSection WebConfigurations
        {
            get
            {
                return webConfigurations;
            }
            set
            {
                webConfigurations = value;
            }
        }

        [XmlElement(ElementName = "RCON")]
        public RCONSettingsSection RCON
        {
            get
            {
                return rcon;
            }
            set
            {
                rcon = value;
            }
        }

        [XmlElement(ElementName = "Server")]
        public IRocketImplementationConfigurationSection Implementation
        {
            get
            {
                return implementatonConfiguration;
            }
            set
            {
                implementatonConfiguration = value;
            }
        }

    }

    public sealed class RocketSettingsManager : MonoBehaviour
    {
        public static Settings Settings;

        private void Awake(){
            Reload();
        }


        public static void Reload()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(RocketSettingsManager));
            string configFile = Path.Combine(RocketBootstrap.Implementation.HomeFolder, "Rocket.config.xml");
            if (File.Exists(configFile))
            {
                using (StreamReader r = new StreamReader(configFile))
                {
                    Settings = (Settings)serializer.Deserialize(r);
                }
                using (StreamWriter w = new StreamWriter(configFile))
                {
                    serializer.Serialize(w, new Settings());
                }
            }
            else
            {
                serializer.Serialize(new StreamWriter(configFile), new RocketSettingsManager());
            }
        }
    }
}