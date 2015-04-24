using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Rocket
{
    public sealed class TextCommand {
        public string Name;
        public string Help;
        [XmlArrayItem("Line")]
        public List<String> Text;
    }

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

        public static List<TextCommand> TextCommands = new List<TextCommand>();
        //public static string[] ChatFilter = new string[] { "cunt", "dick", "pussy", "penis", "vagina", "fuck", "fucking", "fucked", "shit", "shitting", "shat", "damn", "damned", "hell", "cock", "whore", "fag", "faggot", "fag", "nigger" };

        //[XmlArrayItem(ElementName= "ChatFilterListEntry")]
        //[XmlArray(ElementName = "ChatFilterList")]
        //public string[] chatFilter
        //{
        //    get
        //    {
        //        return ChatFilter;
        //    }
        //    set
        //    {
        //        ChatFilter = value;
        //        SDG.ChatManager.ChatFilter = ChatFilter;
        //    }
        //}

        [XmlArrayItem("TextCommand")]
        [XmlArray(ElementName = "TextCommands")]
        public List<TextCommand> textCommands
        {
            get
            {
                return TextCommands;
            }
            set
            {
                TextCommands = value;
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
                    instance.rconPort = s.rconPort;
                    instance.automaticSaveInterval = s.automaticSaveInterval;
                    instance.enableJoinLeaveMessages = s.enableJoinLeaveMessages;
                    instance.automaticShutdownInterval = s.automaticShutdownInterval;
                    instance.automaticShutdownClearLevel = s.automaticShutdownClearLevel;
                    instance.automaticShutdownClearPlayers = s.automaticShutdownClearPlayers;
                    if (s.textCommands != null) instance.textCommands = s.textCommands;
                   // if (s.chatFilter != null) instance.chatFilter = s.chatFilter;
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