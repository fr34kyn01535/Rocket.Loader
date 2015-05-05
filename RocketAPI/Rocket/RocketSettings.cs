using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Rocket
{
    public sealed class RocketSettings
    {
        private static RocketSettings instance;
        public static string HomeFolder;
        public static bool EnableRcon = false;
        public static string RconPassword = "changeme";
        public static int RconPort = 0;
        public static int AutoSaveInterval = 300;

        public static bool EnableJoinLeaveMessages = false;
        public static string LanguageCode = "en";
        public static int AutoShutdownInterval = 0;
        public static bool AutoShutdownClearLevel = false;
        public static bool AutoShutdownClearPlayers = false;

        public static string WebPermissions = "";
        public static string WebConfigurations = "";
        public static int WebPermissionsUpdateInterval = 60;
        
        [XmlElement(ElementName = "WebPermissions")]
        public string webPermissions
        {
            get
            {
                return WebPermissions;
            }
            set
            {
                WebPermissions = value;
            }
        }

        [XmlElement(ElementName = "WebPermissionsUpdateInterval")]
        public int webPermissionsUpdateInterval
        {
            get
            {
                return WebPermissionsUpdateInterval;
            }
            set
            {
                WebPermissionsUpdateInterval = value;
            }
        }

        [XmlElement(ElementName = "WebConfigurations")]
        public string webConfigurations
        {
            get
            {
                return WebConfigurations;
            }
            set
            {
                WebConfigurations = value;
            }
        }

        [XmlElement(ElementName = "AutomaticShutdownInterval")]
        public int automaticShutdownInterval
        {
            get
            {
                return AutoShutdownInterval;
            }
            set
            {
                AutoShutdownInterval = value;
            }
        }

        [XmlElement(ElementName = "AutomaticShutdownClearLevel")]
        public bool automaticShutdownClearLevel
        {
            get
            {
                return AutoShutdownClearLevel;
            }
            set
            {
                AutoShutdownClearLevel = value;
            }
        }

        [XmlElement(ElementName = "AutomaticShutdownClearPlayers")]
        public bool automaticShutdownClearPlayers
        {
            get
            {
                return AutoShutdownClearPlayers;
            }
            set
            {
                AutoShutdownClearPlayers = value;
            }
        }

        [XmlElement(ElementName = "LanguageCode")]
        public string languageCode
        {
            get
            {
                return LanguageCode;
            }
            set
            {
                LanguageCode = value;
            }
        }

        [XmlElement(ElementName = "EnableJoinLeaveMessages")]
        public bool enableJoinLeaveMessages
        {
            get
            {
                return EnableJoinLeaveMessages;
            }
            set
            {
                EnableJoinLeaveMessages = value;
            }
        }

        [XmlElement(ElementName = "EnableRcon")]
        public bool enableRcon
        {
            get
            {
                return EnableRcon;
            }
            set
            {
                EnableRcon = value;
            }
        }

        [XmlElement(ElementName = "RconPassword")]
        public string rconPassword
        {
            get
            {
                return RconPassword;
            }
            set
            {
                RconPassword = value;
            }
        }

        [XmlElement(ElementName = "RconPort")]
        public int rconPort
        {
            get
            {
                return RconPort;
            }
            set
            {
                RconPort = value; ;
            }
        }

        [XmlElement(ElementName = "AutomaticSaveInterval")]
        public int automaticSaveInterval
        {
            get
            {
                return AutoSaveInterval;
            }
            set
            {
                AutoSaveInterval = value; ;
            }
        }

        internal static void LoadSettings()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(RocketSettings));
            string configFile = Path.Combine(HomeFolder, "Rocket.config.xml");
            if (File.Exists(configFile))
            {
                instance = new RocketSettings();
                RocketSettings s;
                using (StreamReader r = new StreamReader(configFile))
                {
                    s = (RocketSettings)serializer.Deserialize(r);
                    instance.enableRcon = s.enableRcon;
                    if (!String.IsNullOrEmpty(s.rconPassword)) instance.rconPassword = s.rconPassword;

                    if (RocketHelper.IsUri(s.webConfigurations)) instance.webConfigurations = s.webConfigurations;
                    if (RocketHelper.IsUri(s.webPermissions)) instance.webPermissions = s.webPermissions;
                    instance.webPermissionsUpdateInterval = s.webPermissionsUpdateInterval;
                    
                    instance.rconPort = s.rconPort;
                    instance.automaticSaveInterval = s.automaticSaveInterval;
                    instance.enableJoinLeaveMessages = s.enableJoinLeaveMessages;
                    instance.automaticShutdownInterval = s.automaticShutdownInterval;
                    instance.automaticShutdownClearLevel = s.automaticShutdownClearLevel;
                    instance.automaticShutdownClearPlayers = s.automaticShutdownClearPlayers;

                }
                using (StreamWriter w = new StreamWriter(configFile))
                {
                    serializer.Serialize(w, instance);
                }
            }
            else
            {
                serializer.Serialize(new StreamWriter(configFile), new RocketSettings());
            }
        }
    }
}