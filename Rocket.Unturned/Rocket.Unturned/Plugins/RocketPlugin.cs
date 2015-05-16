using Rocket.API;
using Rocket.Core.Plugins;
using Rocket.Core.Settings;
using Rocket.Core.Translations;
using Rocket.Unturned.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Rocket.Unturned.Plugins
{

    public class RocketPlugin<TConfiguration> : RocketPlugin, IRocketPlugin<TConfiguration>
    {
        private TConfiguration configuration;
        public TConfiguration Configuration { get { return configuration; } set { configuration = value; } }

        private string homeDirectory;
        public string HomeDirectory { get { return HomeDirectory; } }

        internal override void LoadPlugin()
        {
            ReloadConfiguration();
            base.LoadPlugin();
        }


        public void ReloadConfiguration()
        {
            homeDirectory = Implementation.Instance.HomeFolder + "Plugins/" + (typeof(TConfiguration).Assembly.GetName().Name) + "/";
            if (RocketSettingsManager.Settings.WebConfigurations.Enabled)
            {
                try
                {
                    Configuration = RocketPluginConfiguration.LoadWebConfiguration<TConfiguration>();
                }
                catch (Exception ex)
                {
                    Logger.LogError("Failed to load configuration: " + ex.ToString());
                }
            }
            else
            {
                if (!Directory.Exists(HomeDirectory)) Directory.CreateDirectory(HomeDirectory);

                try
                {
                    Configuration = RocketPluginConfiguration.LoadConfiguration<TConfiguration>(homeDirectory + (typeof(TConfiguration).Assembly.GetName().Name) + ".config.xml");
                }
                catch (Exception ex)
                {
                    Logger.LogError("Failed to load configuration: " + ex.ToString());
                }
            }
        }
    }

    public class RocketPlugin : MonoBehaviour, IRocketPlugin
    {
        private bool loaded = false;
        public bool Loaded { get { return loaded; } }

        private Dictionary<string, string> translations;
        public Dictionary<string, string> Translations { get { return translations; } }

        public virtual Dictionary<string, string> DefaultTranslations { get { return new Dictionary<string, string>(); } }

        internal virtual void LoadPlugin()
        {
            DontDestroyOnLoad(transform.gameObject);
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
            loaded = true;
            try
            {
                Load();
            }
            catch (Exception ex)
            {
                string name = GetType().Assembly.GetName().Name;
                Logger.LogError("Failed to load " + name + ", unloading now... :" + ex.ToString());
                try
                {
                    UnloadPlugin();
                }
                catch (Exception ex1)
                {
                    Logger.LogError("Failed to unload " + name + ":" + ex1.ToString());
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
            translations = RocketTranslationManager.LoadTranslation(name, DefaultTranslations);
        }

        internal virtual void UnloadPlugin()
        {
            Unload();
            PluginManager.UnregisterCommands(GetType().Assembly);
            PluginManager.RemoveRocketPlayerComponents(GetType().Assembly);
            loaded = false;
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

        public void Load()
        {

        }


        public void Unload()
        {

        }
    }
}