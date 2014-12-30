using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Rocket.RocketAPI
{
    public class RocketConfiguration
    {
        private static string configFile = "{0}Plugins/{1}.config";

        public static void SaveConfiguration<T>(bool overwrite = true){
            string filename = String.Format(configFile, Bootstrap.HomeFolder, typeof(T).Assembly.GetName().Name);

            if (!Directory.Exists(Path.GetDirectoryName(filename)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filename));
            }

            if (!File.Exists(filename) && overwrite)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (TextWriter writer = new StreamWriter(filename))
                {
                    serializer.Serialize(writer, Activator.CreateInstance(typeof(T)));
                }
            }
        }

        public static T LoadConfiguration<T>()
        {
            string filename = String.Format(configFile, Bootstrap.HomeFolder, typeof(T).Assembly.GetName().Name);
            if (File.Exists(filename))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(new StreamReader(filename));
            }
            else
            {
                SaveConfiguration<T>();
                return (T)Activator.CreateInstance(typeof(T));
            }
        }
    }
}
