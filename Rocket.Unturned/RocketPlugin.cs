using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Rocket.API
{

    public class RocketPlugin<TConfiguration> : RocketPlugin
    {
        public TConfiguration Configuration;
        public string HomeDirectory;

        internal override void LoadPlugin()
        {
            ReloadConfiguration();
            base.LoadPlugin();
        }


        public void ReloadConfiguration()
        {
            if (SettingsManager.WebConfigurations.Enabled)
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
            else
            {
                HomeDirectory = Rocket.HomeFolder + "Plugins/" + (typeof(TConfiguration).Assembly.GetName().Name);
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
        }
    }

    public class RocketPlugin : MonoBehaviour
    {
        public bool Loaded = false;
        public Dictionary<string, string> Translations = null;
        public virtual Dictionary<string, string> DefaultTranslations { get { return new Dictionary<string, string>(); } }

        internal void Awake() {
            DontDestroyOnLoad(transform.gameObject);
        }

        internal virtual void LoadPlugin()
        {
            try
            {
                PluginManager.AddRocketPlayerComponents(GetType().Assembly);
                PluginManager.RegisterCommands(GetType().Assembly);
                ReloadTranslation();
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
                string name = GetType().Assembly.GetName().Name;
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

        public void ReloadTranslation()
        {
            string name = GetType().Assembly.GetName().Name;
#if DEBUG
            int c = DefaultTranslations == null ? 0 : DefaultTranslations.Count;
            Logger.Log("Loading " + c + " translations for " + name);
#endif
            Translations = TranslationHelper.LoadTranslation(name, DefaultTranslations);
        }

        internal virtual void UnloadPlugin()
        {
            Unload();
            PluginManager.UnregisterCommands(GetType().Assembly);
            PluginManager.RemoveRocketPlayerComponents(GetType().Assembly);
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