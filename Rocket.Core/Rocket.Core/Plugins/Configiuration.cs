using Rocket.API;
using Rocket.Core;
using Rocket.Core.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Rocket.Unity
{

    public static class PluginConfiguration
    {
        private static string configurationFile = "{0}Plugins/{1}/{1}.config.xml";

        public static void Save(this IRocketConfiguration configuration)
        {
            string filename = String.Format(configurationFile, Rocket.HomeFolder, configuration.GetType().Assembly.GetName().Name);
            PluginConfiguration.SaveConfiguration(configuration,filename);
        }

        internal static T LoadWebConfiguration<T>()
        {
            string target = SettingsManager.Settings.WebConfigurations.Url;
            if (String.IsNullOrEmpty(target)) return default(T);
            try
            {
                if (target.Contains("?"))
                {
                    target += "&";
                }
                else
                {
                    target += "?";
                }

                target += "configuration=" + typeof(T).Assembly.GetName().Name + "&instance=" + Steam.InstanceName + "&request=" + Guid.NewGuid();
                string xml = new WebClient().DownloadString(target);
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                T output = (T)serializer.Deserialize(new StringReader(xml));
                return output;
            }
            catch (Exception ex)
            {
                Logger.LogError("An error occured while loading the configuration from " + target + ": " + ex.ToString());
                return (T)Activator.CreateInstance(typeof(T));
            }
        }

        internal static T LoadConfiguration<T>(string filename)
        {
            if (File.Exists(filename))
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));

                    T output = default(T);

                    using (StreamReader r = new StreamReader(filename))
                    {
                        output = (T)serializer.Deserialize(r);
                    }

                    XmlSerializer outserializer = new XmlSerializer(typeof(T));

                    using (TextWriter writer = new StreamWriter(filename, false))
                    {
                        outserializer.Serialize(writer, output);
                    }

                    return output;
                }
                catch (Exception ex)
                {
                    Logger.LogError("An error occured while loading the configuration: " + ex.ToString());// The old version was backuped and a new version was created: " + ex.ToString());
                    return (T)Activator.CreateInstance(typeof(T));
                }
            }
            else
            {
                return InitialiseConfiguration<T>(filename);
            }
        }

        internal static T InitialiseConfiguration<T>(string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            T config = (T)Activator.CreateInstance(typeof(T));
            using (TextWriter writer = new StreamWriter(filename))
            {
                if (typeof(T).GetInterfaces().Contains(typeof(IRocketConfiguration)))
                {
                    config = (T)((IRocketConfiguration)config).DefaultConfiguration;
                }
                serializer.Serialize(writer, config);
            }
            return config;
        }

        internal static void SaveConfiguration(IRocketConfiguration configuration,string filename)
        {
            if (SettingsManager.Settings.WebConfigurations.Enabled) return;
            XmlSerializer serializer = new XmlSerializer(configuration.GetType());

            using (TextWriter writer = new StreamWriter(filename))
            {
                serializer.Serialize(writer, configuration);
            }
        }
    }
}
