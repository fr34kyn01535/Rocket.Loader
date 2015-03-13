using Rocket.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Rocket.RocketAPI
{
    public static class RocketTranslationHelper
    {
        public class Translation
        {
            [XmlAttribute]
            public string Id;
            [XmlAttribute]
            public string Value;
        }

        private static string configurationFile = "{0}Plugins/{1}/{1}.{2}.translation.xml"; // Plugins/TestPlugin/TestPlugin.en.translation.xml
        internal static Dictionary<string, string> LoadTranslation(string assemblyName,Dictionary<string, string> fallback)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Translation[]), new XmlRootAttribute() { ElementName = "Translations" });
            Dictionary<string, string> translations;
            string rocketTranslation = String.Format(configurationFile, RocketSettings.HomeFolder, assemblyName, RocketSettings.LanguageCode);

            if (File.Exists(rocketTranslation))
            {
                using (StreamReader r = new StreamReader(rocketTranslation))
                {
                    translations = ((Translation[])serializer.Deserialize(r)).ToDictionary(i => i.Id, i => i.Value);
                }
                foreach (string key in fallback.Keys) {
                    if (!translations.ContainsKey(key)) {
                        translations.Add(key, fallback[key]);
                    }
                }
            }
            else
            {
                if (RocketSettings.LanguageCode != "en")
                {
                    rocketTranslation = String.Format(configurationFile, RocketSettings.HomeFolder, assemblyName, "en");
                    Logger.LogWarning(Path.GetFileName(rocketTranslation) + " could not be found, recovering default language");
                }
                translations = fallback;
            }
            if (translations.Count != 0)
            {
                using (StreamWriter w = new StreamWriter(rocketTranslation))
                {
                    serializer.Serialize(w, translations.Select(kv => new Translation() { Id = kv.Key, Value = kv.Value }).ToArray());
                }
            }
            return translations;
        }
    }
}
