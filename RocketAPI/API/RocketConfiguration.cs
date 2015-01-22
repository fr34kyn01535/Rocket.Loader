using SDG;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

namespace Rocket.RocketAPI
{
    public interface RocketConfiguration
    {
        RocketConfiguration DefaultConfiguration { get; }
    }

    internal static class RocketConfigurationHelper
    {
        public static T LoadConfiguration<T>()
        {
            string filename = String.Format("{0}Plugins/{1}/{1}.config", RocketSettings.HomeFolder, typeof(T).Assembly.GetName().Name);
            if (File.Exists(filename))
            {
                try
                {
                    string filecontent = "";
                    using (StreamReader reader = new StreamReader(filename)) { 
                        filecontent = reader.ReadToEnd().Trim();
                    }

                    Uri uriOut = null;
                    if (Uri.TryCreate(filecontent, UriKind.Absolute, out uriOut) && (uriOut.Scheme == Uri.UriSchemeHttp || uriOut.Scheme == Uri.UriSchemeHttps)) {

                        string target = uriOut.ToString();

                        if (target.Contains("?"))
                        {
                            target += "&";
                        }
                        else
                        {
                            target += "?";
                        }

                        target += "configuration=" + typeof(T).Assembly.GetName().Name + "&instance=" + Steam.Servername+"&request="+Guid.NewGuid();
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
                    Logger.LogError("An error occured while loading the configuration. The old version was backuped and a new version was created: " + ex.ToString());
                    File.Copy(filename, filename + ".bak", true);
                    return SaveConfiguration<T>(filename);
                }
            }
            else
            {
                return SaveConfiguration<T>(filename);
            }
        }
        public static T SaveConfiguration<T>(string filename) {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (TextWriter writer = new StreamWriter(filename))
            {
                object config = Activator.CreateInstance(typeof(T));
                if (typeof(T).GetInterfaces().Contains(typeof(RocketConfiguration)))
                {
                    config = ((RocketConfiguration)config).DefaultConfiguration;
                }
                serializer.Serialize(writer, config);
            }
            return (T)Activator.CreateInstance(typeof(T));
        }
    }
}
