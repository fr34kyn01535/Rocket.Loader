using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

namespace Rocket.RocketAPI
{
    public class RocketConfiguration
    {
        public static T LoadConfiguration<T>()
        {
            string filename = String.Format("{0}Plugins/{1}.config",RocketSettings.HomeFolder, typeof(T).Assembly.GetName().Name);
            if (File.Exists(filename))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));

                T output = default(T);

                using (StreamReader reader = new StreamReader(filename))
                {
                    output = (T)serializer.Deserialize(reader);
                }

                XmlSerializer outserializer = new XmlSerializer(typeof(T));

               /* using (TextWriter writer = new StreamWriter(filename,false))
                {
                    outserializer.Serialize(writer, output);
                }*/

                return output;
            }
            else
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (TextWriter writer = new StreamWriter(filename))
                {
                    object config = Activator.CreateInstance(typeof(T));
                    serializer.Serialize(writer, config);
                }
                return (T)Activator.CreateInstance(typeof(T));
            }
        }
    }
}
