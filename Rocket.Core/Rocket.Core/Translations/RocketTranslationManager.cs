using Rocket.Core.Logging;
using Rocket.Core.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;


namespace Rocket.Core.Translations
{
    public class RocketTranslationManager : MonoBehaviour
    {
        public class TranslationEntry
        {
            [XmlAttribute]
            public string Id;
            [XmlAttribute]
            public string Value;
        }
        internal void Awake() {
            foreach (var translation in RocketBootstrap.Implementation.Translation)
                defaultTranslations.Add(translation.Key, translation.Value);
            Reload();
        }

        private static string pluginTranslationFile = "{0}Plugins/{1}/{1}.{2}.translation.xml";
        private static string pluginFolder = "{0}Plugins/{1}/";

        private static string translationFile = "{0}/Rocket.{1}.translation.xml";

        #region defaultTranslations
        private static Dictionary<string, string> defaultTranslations = new Dictionary<string, string>()
        { 
            {"rocket_join_public","{0} connected to the server"},
            {"rocket_leave_public","{0} disconnected from the server"},
            {"rocket_restart_warning_public","This server will be restarted in 30 seconds"}
        };
        #endregion
        
        private static Dictionary<string, string> translations = null;

        public static string Translate(string translationKey, params object[] placeholder)
        {
            try
            {
                string value = null;
                if (translations != null)
                {
                    translations.TryGetValue(translationKey, out value);
                    if (value == null) value = translationKey;

                    for (int i = 0; i < placeholder.Length; i++)
                    {
                        if (placeholder[i] == null) placeholder[i] = "NULL";
                    }

                    if (value.Contains("{0}") && placeholder != null && placeholder.Length != 0)
                    {
                        value = String.Format(value, placeholder);
                    }
                }
                return value;
            }
            catch (Exception er)
            {
                Logger.LogError("Error fetching translation for " + translationKey+": "+er.ToString());
                return translationKey;
            }
        }

        public static void Reload()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(TranslationEntry[]), new XmlRootAttribute() { ElementName = "Translations" });
                string rocketTranslation = String.Format(translationFile, RocketBootstrap.Implementation.HomeFolder, RocketSettingsManager.Settings.LanguageCode);

                if (File.Exists(rocketTranslation))
                {
                    using (StreamReader r = new StreamReader(rocketTranslation))
                    {
                        translations = ((TranslationEntry[])serializer.Deserialize(r)).ToDictionary(i => i.Id, i => i.Value);
                    }
                    foreach (string key in defaultTranslations.Keys)
                    {
                        if (!translations.ContainsKey(key))
                        {
                            translations.Add(key, defaultTranslations[key]);
                        }
                    }
                }
                else
                {
                    if (RocketSettingsManager.Settings.LanguageCode != "en")
                    {
                        Logger.LogWarning(Path.GetFileName(rocketTranslation) + " could not be found, recovering default language");
                        rocketTranslation = String.Format(translationFile, RocketBootstrap.Implementation.HomeFolder, "en");
                    }
                    translations = defaultTranslations;
                }

                using (StreamWriter w = new StreamWriter(rocketTranslation))
                {
                    serializer.Serialize(w, translations.Select(kv => new TranslationEntry() { Id = kv.Key, Value = kv.Value }).ToArray());
                }

            }
            catch (Exception ex)
            {
                Logger.LogError("Error loading translations: "+ex.ToString());
            }
        }

        public static Dictionary<string, string> LoadTranslation(string assemblyName, Dictionary<string, string> fallback)
        {
            if (!Directory.Exists(String.Format(pluginFolder, RocketBootstrap.Implementation.HomeFolder, assemblyName))) return fallback;
            XmlSerializer serializer = new XmlSerializer(typeof(TranslationEntry[]), new XmlRootAttribute() { ElementName = "Translations" });
            Dictionary<string, string> translations;
            string rocketTranslation = String.Format(pluginTranslationFile, RocketBootstrap.Implementation.HomeFolder, assemblyName, RocketSettingsManager.Settings.LanguageCode);

            if (File.Exists(rocketTranslation))
            {
                using (StreamReader r = new StreamReader(rocketTranslation))
                {
                    translations = ((TranslationEntry[])serializer.Deserialize(r)).ToDictionary(i => i.Id, i => i.Value);
                }
                foreach (string key in fallback.Keys)
                {
                    if (!translations.ContainsKey(key))
                    {
                        translations.Add(key, fallback[key]);
                    }
                }
            }
            else
            {
                if (RocketSettingsManager.Settings.LanguageCode != "en")
                {
                    rocketTranslation = String.Format(pluginTranslationFile, RocketBootstrap.Implementation.HomeFolder, assemblyName, "en");
                    Logger.LogWarning(Path.GetFileName(rocketTranslation) + " could not be found, recovering default language");
                }
                translations = fallback;
            }
            if (translations.Count != 0)
            {
                using (StreamWriter w = new StreamWriter(rocketTranslation))
                {
                    serializer.Serialize(w, translations.Select(kv => new TranslationEntry() { Id = kv.Key, Value = kv.Value }).ToArray());
                }
            }
            return translations;
        }
    }
}
