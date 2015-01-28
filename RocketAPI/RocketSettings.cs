using System.IO;
using System.Xml.Serialization;

namespace Rocket
{
    public class RocketSettings
    {
        private static RocketSettings instance;
        public static string HomeFolder;
        public static bool EnableRcon = false;
        public static string RconPassword = "changeme";
        public static int RconPort = 0;

        [XmlElement(ElementName = "EnableRcon")]
        public bool enableRcon
        {
            get
            {
                return EnableRcon;
            }
            set
            {
                EnableRcon = value; ;
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
                RconPassword = value; ;
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

        internal static void LoadSettings()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(RocketSettings));
            string configFile = Path.Combine(HomeFolder, "Rocket.config");
            if (File.Exists(configFile))
            {
                instance = new RocketSettings();
                RocketSettings s = (RocketSettings)serializer.Deserialize(new StreamReader(configFile));
                instance.enableRcon = s.enableRcon;
                if (s.rconPassword != null) instance.rconPassword = s.rconPassword;
                instance.rconPort = s.rconPort;
            }
            else
            {
                serializer.Serialize(new StreamWriter(configFile), new RocketSettings());
            }
        }
    }
}