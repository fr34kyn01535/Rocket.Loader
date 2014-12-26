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
        private static string configFile = "./Unturned_Data/Managed/Plugins/{0}/{1}.config";

        public void SaveConfiguration(bool overwrite = true){
            Type type = this.GetType();
            string filename = String.Format(configFile, Bootstrap.InstanceName,type.Assembly.GetName().Name);

            if (!Directory.Exists(Path.GetDirectoryName(filename)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filename));
            }

            if (!File.Exists(filename) && overwrite)
            {
                XmlSerializer serializer = new XmlSerializer(type);
                using (TextWriter writer = new StreamWriter(filename))
                {
                    serializer.Serialize(writer, Activator.CreateInstance(type));
                }
            }
        }

        public object LoadConfiguration()
        {
            Type type = this.GetType();
            string filename = String.Format(configFile, Bootstrap.InstanceName, type.Assembly.GetName().Name);
            if (File.Exists(filename))
            {
                XmlSerializer serializer = new XmlSerializer(type);
                return serializer.Deserialize(new StreamReader(filename));
            }
            else
            {
                SaveConfiguration();
                return Activator.CreateInstance(type);
            }
        }
    }
}
