using Rocket.Components;
using Rocket.Logging;
using SDG;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Rocket.RocketAPI
{

    public class RocketPlugin<TConfiguration> : RocketPlugin
    {
        public TConfiguration Configuration;
        public string HomeDirectory;

        internal override void LoadPlugin()
        {
            if (String.IsNullOrEmpty(RocketSettings.WebConfigurations))
            {
                HomeDirectory = RocketSettings.HomeFolder + "Plugins/" + (typeof(TConfiguration).Assembly.GetName().Name);
                if (!Directory.Exists(HomeDirectory)) Directory.CreateDirectory(HomeDirectory);

                try
                {
                    Configuration = RocketConfiguration.LoadConfiguration<TConfiguration>();
                }
                catch (Exception ex)
                {
                    Logger.LogError("Failed to load configuration: " + ex.ToString());
                }
            }
            else
            {
                try
                {
                    Configuration = RocketConfiguration.LoadWebConfiguration<TConfiguration>();
                }
                catch (Exception ex)
                {
                    Logger.LogError("Failed to load configuration: " + ex.ToString());
                }
            }
            base.LoadPlugin();
        }
    }

    public class RocketPlugin : RocketManagerComponent
    {
        public bool Loaded = false;
        public Dictionary<string, string> Translations = null;
        public virtual Dictionary<string, string> DefaultTranslations { get { return new Dictionary<string, string>(); } }

        internal virtual void LoadPlugin()
        {
            DontDestroyOnLoad(transform.gameObject);
            string name = GetType().Assembly.GetName().Name;
            try
            {
                RocketPluginManager.AddRocketPlayerComponents(GetType().Assembly);
                RocketPluginManager.RegisterCommands(GetType().Assembly);


#if DEBUG
                int c = DefaultTranslations == null ? 0 : DefaultTranslations.Count;
                Logger.Log("Loading " + c + " translations for " + name);
#endif
                Translations = RocketTranslationHelper.LoadTranslation(name, DefaultTranslations);
            }
            catch (Exception ex)
            {
                Logger.LogError("Failed to load translation: " + ex.ToString());
            }
            Loaded = true;
            try
            {
                Load();
            }
            catch (Exception ex)
            {
                Logger.LogError("Failed to load " + name+", unloading now... :"+ex.ToString());
                try 
	            {	        
                    UnloadPlugin();
	            }
	            catch (Exception ex1)
	            {
                    Logger.LogError("Failed to unload " + name+":"+ex1.ToString());
	            }
            }
        }


        internal virtual void UnloadPlugin()
        {
            Unload();
            RocketPluginManager.UnregisterCommands(GetType().Assembly);
            RocketPluginManager.RemoveRocketPlayerComponents(GetType().Assembly);
            Loaded = false;
        }

        internal void OnEnable()
        {
            LoadPlugin();
        }

        internal void OnDisable()
        {
            UnloadPlugin();
        }


        public string Translate(string translationKey, params object[] placeholder)
        {
            try
            {
                string value = translationKey;
                if (Translations != null)
                {
                    Translations.TryGetValue(translationKey, out value);

                    for (int i = 0; i < placeholder.Length; i++)
                    {
                        if (placeholder[i] == null) placeholder[i] = "NULL";
                    }

                    if (value != null && value.Contains("{0}") && placeholder != null && placeholder.Length != 0)
                    {
                        value = String.Format(value, placeholder);
                    }
                }
                return value;
            }
            catch (Exception er)
            {
                Logger.LogError("Error fetching translation for " + translationKey + ": " + er.ToString());
                return translationKey;
            }
        }

        protected virtual void Load()
        {

        }


        protected virtual void Unload()
        {

        }
    }
}