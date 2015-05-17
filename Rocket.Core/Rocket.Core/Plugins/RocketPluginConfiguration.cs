﻿using Rocket.API;
using Rocket.Core.Logging;
using Rocket.Core.Misc;
using Rocket.Core.Settings;
using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Rocket.Core.Plugins
{

    public static class RocketPluginConfiguration
    {
        public static T LoadWebConfiguration<T>()
        {
            string target = RocketSettingsManager.Settings.WebConfigurations.Url;
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

                target += "configuration=" + typeof(T).Assembly.GetName().Name + "&instance=" + RocketBootstrap.Implementation.InstanceName + "&request=" + Guid.NewGuid();
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

        public static T LoadConfiguration<T>(string filename)
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
                return initialiseConfiguration<T>(filename);
            }
        }

        private static T initialiseConfiguration<T>(string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            T config = (T)Activator.CreateInstance(typeof(T));
            using (TextWriter writer = new StreamWriter(filename))
            {
                if (typeof(T).GetInterfaces().Contains(typeof(IRocketPluginConfiguration)))
                {
                    config = (T)((IRocketPluginConfiguration)config).DefaultConfiguration;
                }
                serializer.Serialize(writer, config);
            }
            return config;
        }

        public static void SaveConfiguration(IRocketPluginConfiguration configuration, string filename)
        {
            if (RocketSettingsManager.Settings.WebConfigurations.Enabled) return;
            XmlSerializer serializer = new XmlSerializer(configuration.GetType());

            using (TextWriter writer = new StreamWriter(filename))
            {
                serializer.Serialize(writer, configuration);
            }
        }
    }
}
