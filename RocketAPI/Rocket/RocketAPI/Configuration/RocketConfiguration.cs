using Rocket.Logging;
using SDG;
using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Rocket.RocketAPI
{
    public interface RocketConfiguration //Not correct! TODO: FIX THIS ON MAJOR UPDATE
    {
        RocketConfiguration DefaultConfiguration { get; }
    }


    public static class RocketConfigurationHelper
    {
        private static string configurationFile = "{0}Plugins/{1}/{1}.config.xml";

        public static void Save(this RocketConfiguration configuration)
        {
            RocketConfigurationHelper.SaveConfiguration(configuration);
        }

        private static Uri getUriFromFile(string filename)
        {
            string filecontent = "";
            using (StreamReader reader = new StreamReader(filename))
            {
                filecontent = reader.ReadToEnd().Trim();
            }
            Uri uriOut = null;
            if (Uri.TryCreate(filecontent, UriKind.Absolute, out uriOut) && (uriOut.Scheme == Uri.UriSchemeHttp || uriOut.Scheme == Uri.UriSchemeHttps))
            {
                return uriOut;
            }
            return null;
        }

        public static T LoadConfiguration<T>()
        {
            string filename = String.Format(configurationFile, RocketSettings.HomeFolder, typeof(T).Assembly.GetName().Name);
            if (File.Exists(filename))
            {
                try
                {
                    string filecontent = "";
                    using (StreamReader reader = new StreamReader(filename))
                    {
                        filecontent = reader.ReadToEnd().Trim();
                    }
                    Uri uriOut = getUriFromFile(filename);
                    if (uriOut!=null)
                    {
                        string target = uriOut.ToString();

                        if (target.Contains("?"))
                        {
                            target += "&";
                        }
                        else
                        {
                            target += "?";
                        }

                        target += "configuration=" + typeof(T).Assembly.GetName().Name + "&instance=" + Steam.InstanceName + "&request=" + Guid.NewGuid();
                        filecontent = new RocketWebClient().DownloadString(target);
                    }

                    XmlSerializer serializer = new XmlSerializer(typeof(T));

                    T output = default(T);

                    output = (T)serializer.Deserialize(new StringReader(filecontent));

                    if (uriOut == null)
                    {
                        XmlSerializer outserializer = new XmlSerializer(typeof(T));

                        using (TextWriter writer = new StreamWriter(filename, false))
                        {
                            outserializer.Serialize(writer, output);
                        }
                    }

                    return output;
                }
                catch (Exception ex)
                {
                    Logger.LogError("An error occured while loading the configuration: " + ex.ToString());// The old version was backuped and a new version was created: " + ex.ToString());
                    //File.Copy(filename, filename + ".bak", true);
                    //return SaveConfiguration<T>(filename);
                    return (T)Activator.CreateInstance(typeof(T));
                }
            }
            else
            {
                return InitialiseConfiguration<T>();
            }
        }

        public static T InitialiseConfiguration<T>()
        {
            string filename = String.Format(configurationFile, RocketSettings.HomeFolder, typeof(T).Assembly.GetName().Name);
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            T config = (T)Activator.CreateInstance(typeof(T));
            using (TextWriter writer = new StreamWriter(filename))
            {
                if (typeof(T).GetInterfaces().Contains(typeof(RocketConfiguration)))
                {
                    config = (T)((RocketConfiguration)config).DefaultConfiguration;
                }
                serializer.Serialize(writer, config);
            }
            return config;
        }

        public static void SaveConfiguration(RocketConfiguration configuration)
        {
            string filename = String.Format(configurationFile, RocketSettings.HomeFolder, configuration.GetType().Assembly.GetName().Name);

            if (getUriFromFile(filename) != null)
            {
                return;
            }

            XmlSerializer serializer = new XmlSerializer(configuration.GetType());

            using (TextWriter writer = new StreamWriter(filename))
            {
                serializer.Serialize(writer, configuration);
            }
        }
    }
}