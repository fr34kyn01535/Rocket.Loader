using Rocket.Components;
using Rocket.Logging;
using System;
using System.Collections.Generic;
using System.IO;

namespace Rocket.RocketAPI
{

    public class RocketPlugin<TConfiguration> : RocketPlugin
    {
        public static TConfiguration Configuration;
        public static string HomeDirectory;

        public override void Awake()
        {
            HomeDirectory = RocketSettings.HomeFolder + "Plugins/" + (typeof(TConfiguration).Assembly.GetName().Name);
            if (!Directory.Exists(HomeDirectory)) Directory.CreateDirectory(HomeDirectory);

            try
            {
                Configuration = RocketConfigurationHelper.LoadConfiguration<TConfiguration>();
            }
            catch (Exception ex)
            {
                Logger.LogError("Failed to load configuration: " + ex.ToString());
            }

            base.Awake();
        }
    }

    public class RocketPlugin : RocketManagerComponent
    {
        public static bool Loaded = false;
        public static Dictionary<string, string> Translations = null;
        public virtual Dictionary<string, string> DefaultTranslations { get { return new Dictionary<string, string>(); } }

        public void Start()
        {
            Load();
        }

        public virtual void Awake()
        {
            DontDestroyOnLoad(transform.gameObject);
            
            try
            {
#if DEBUG
                int c = DefaultTranslations == null ? 0 : DefaultTranslations.Count;
                Logger.Log("Loading " + c + " translations for " + this.GetType().Assembly.GetName().Name);
#endif
                Translations = RocketTranslationHelper.LoadTranslation(this.GetType().Assembly.GetName().Name, DefaultTranslations);
            }
            catch (Exception ex)
            {
                Logger.LogError("Failed to load translation: " + ex.ToString());
            }

            Loaded = true;
        }

        public static string Translate(string translationKey, params object[] placeholder)
        {
            string value = null;
            if (Translations != null)
            {
                Translations.TryGetValue(translationKey, out value);
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

        protected virtual void Load()
        {
        }
    }
}