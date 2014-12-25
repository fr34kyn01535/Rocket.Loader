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
        private static string configFile = "./Unturned_Data/Managed/Plugins/{0}.config";


        public void SaveConfiguration(bool overwrite = true){
            Type type = this.GetType();
            string filename = String.Format(configFile, type.Assembly.GetName().Name);

            XmlSerializer serializer = new XmlSerializer(type);

            if (!File.Exists(filename) && overwrite)
            {
                using (TextWriter writer = new StreamWriter(filename))
                {
                    serializer.Serialize(writer, Activator.CreateInstance(type));
                }
            }
        }

        public object LoadConfiguration()
        {
            Type type = this.GetType();
            string filename = String.Format(configFile, type.Assembly.GetName().Name);
            if (File.Exists(filename))
            {
                SaveConfiguration();
                XmlSerializer serializer = new XmlSerializer(type);
                return serializer.Deserialize(new StreamReader(filename));
            }
            else {
                return Activator.CreateInstance(type);
            }
        }
    }
}
