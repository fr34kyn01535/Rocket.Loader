using Rocket.RocketAPI.Commands;
using Rocket.RocketAPI.Interfaces;
using SDG;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace Rocket.RocketAPI.Managers
{
    public class ConfigurationManager
    {
        private static string configFile = "{0}Plugins/{1}.config";
        private static void saveConfiguration<T>(bool overwrite = true)
        {
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
        /// <summary>
        /// This method allowes to load the configuration from file
        /// </summary>
        /// <typeparam name="T">A type that inherits from RocketConfiguration and has all the custom configuration options members</typeparam>
        /// <returns>Tha class with the values set either to the values from the config, or if not found set to default</returns>
        public static T LoadConfiguration<T>()
        {
            string filename = String.Format(configFile, Bootstrap.HomeFolder, typeof(T).Assembly.GetName().Name);
            if (File.Exists(filename))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));

                T output = default(T);

                using (StreamReader reader = new StreamReader(filename))
                {
                    output = (T)serializer.Deserialize(reader);
                }

                /* using (TextWriter writer = new StreamWriter(filename))
                 {
                     serializer.Serialize(writer, output);
                 }
                 */
                return output;
            }
            else
            {
                saveConfiguration<T>();
                return (T)Activator.CreateInstance(typeof(T));
            }
        }
    }
}
