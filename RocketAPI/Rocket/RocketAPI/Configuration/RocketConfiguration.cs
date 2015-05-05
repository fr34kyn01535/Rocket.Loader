using Rocket.Logging;
using SDG;
using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Rocket.RocketAPI
{
    public interface IRocketConfiguration
    {
        IRocketConfiguration DefaultConfiguration { get; }
    }


    internal static class RocketConfigurationHelper
    {
        private static string configurationFile = "{0}Plugins/{1}/{1}.config.xml";

        public static void Save(this IRocketConfiguration configuration)
        {
            RocketConfigurationHelper.SaveConfiguration(configuration);
        }

        internal static T LoadWebConfiguration<T>()
        {
            string target = RocketSettings.WebConfigurations;
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
                string xml = new RocketWebClient().DownloadString(target);
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

        internal static T LoadConfiguration<T>()
        {
            string filename = String.Format(configurationFile, RocketSettings.HomeFolder, typeof(T).Assembly.GetName().Name);
            if (File.Exists(filename))
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));

                    T output = default(T);

                    output = (T)serializer.Deserialize(new StreamReader(filename));

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
                return InitialiseConfiguration<T>();
            }
        }

        internal static T InitialiseConfiguration<T>()
        {
            string filename = String.Format(configurationFile, RocketSettings.HomeFolder, typeof(T).Assembly.GetName().Name);
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

        internal static void SaveConfiguration(IRocketConfiguration configuration)
        {
            if (!String.IsNullOrEmpty(RocketSettings.WebConfigurations)) return;
            string filename = String.Format(configurationFile, RocketSettings.HomeFolder, configuration.GetType().Assembly.GetName().Name);
            XmlSerializer serializer = new XmlSerializer(configuration.GetType());

            using (TextWriter writer = new StreamWriter(filename))
            {
                serializer.Serialize(writer, configuration);
            }
        }
    }
}