using Rocket.API;
using Rocket.Core.Logging;
using System;
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

    public sealed class RocketSettings{
        
        private RCONSettingsSection rcon = new RCONSettingsSection();
        private AutomaticShutdownSettingsSection automaticShutdown = new AutomaticShutdownSettingsSection();

        private WebConfigurationsSettingsSection webConfigurations = new WebConfigurationsSettingsSection();
        private WebPermissionsSettingsSection webPermissions = new WebPermissionsSettingsSection();

        private bool enableJoinLeaveMessages = false;
        //private bool enableSettingsWatcher = true;
        private string languageCode = "en";
        private int automaticSaveInterval = 300;

        private object implementatonConfiguration = RocketBootstrap.Implementation.Configuration;


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


        //[XmlElement(ElementName = "EnableSettingsWatcher")]
        //public bool EnableSettingsWatcher
        //{
        //    get
        //    {
        //        return enableSettingsWatcher;
        //    }
        //    set
        //    {
        //        enableSettingsWatcher = value;
        //    }
        //}

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

        [XmlElement(ElementName = "Game")]
        public object Implementation
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
        public static RocketSettings Settings;


        private void Awake()
        {
#if DEBUG
            Logger.Log("RocketSettingsManager > Awake");
#endif
            Reload();
        }


        public static void Reload(bool writeAgain = true)
        {
            try
            {
                RocketSettings fallback = new RocketSettings();
                Type[] types = {fallback.Implementation.GetType()};
                XmlSerializer serializer = new XmlSerializer(typeof(RocketSettings), types);
                string configFile = Path.Combine(RocketBootstrap.Implementation.HomeFolder, RocketBootstrap.SettingsFile);
                if (File.Exists(configFile))
                {
                    using (StreamReader r = new StreamReader(configFile))
                    {
                        Settings = (RocketSettings)serializer.Deserialize(r);
                    }
                    if (writeAgain)
                    {
                        using (StreamWriter w = new StreamWriter(configFile))
                        {
                            serializer.Serialize(w, Settings);
                        }
                    }
                }
                else
                {
                    Settings = fallback;
                    serializer.Serialize(new StreamWriter(configFile), fallback);
                }
            }
            catch (System.Exception ex)
            {
                Logger.LogError("Error loading RocketSettings: " + ex.ToString());
            }
        }
    }
}