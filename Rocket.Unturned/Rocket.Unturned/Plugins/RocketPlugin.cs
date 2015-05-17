using Rocket.API;
using Rocket.Core.Plugins;
using Rocket.Core.Settings;
using Rocket.Core.Tasks;
using Rocket.Core.Translations;
using Rocket.Unturned.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Rocket.Unturned.Plugins
{
    public static class RocketPluginExtensions
    {
        public static void Save(this IRocketPluginConfiguration configuration)
        {
            string filename = String.Format("{1}/{1}.config.xml", Implementation.Instance.PluginsFolder, configuration.GetType().Assembly.GetName().Name);
            RocketPluginConfiguration.SaveConfiguration(configuration, filename);
        }
    }

    public class RocketPlugin<TConfiguration> : RocketPlugin, IRocketPlugin<TConfiguration>
    {
        private TConfiguration configuration;
        public TConfiguration Configuration { get { return configuration; } set { configuration = value; } }

        private string homeDirectory;
        public string HomeDirectory { get { return HomeDirectory; } }

        internal override void LoadPlugin()
        {
            try
            {
                ReloadConfiguration(); 
            }
            catch (Exception ex)
            {
                Logger.LogError("Failed to load configuration: " + ex.ToString());
            }
            base.LoadPlugin();
        }

        public void ReloadConfiguration()
        {
            homeDirectory = Implementation.Instance.PluginsFolder + (typeof(TConfiguration).Assembly.GetName().Name) + "/";
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
                if (!Directory.Exists(homeDirectory)) Directory.CreateDirectory(homeDirectory);

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

        public static void EnqueueTask(Action a)
        {
            RocketTaskManager.Enqueue(a);
        }

        internal virtual void LoadPlugin()
        {
            DontDestroyOnLoad(transform.gameObject);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine();
            Console.WriteLine(("Loading " + GetType().Name +" "+ GetType().Assembly.GetName().Version.ToString()).PadRight(80, '.'));
            Console.ForegroundColor = ConsoleColor.White;
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

            Console.ForegroundColor = ConsoleColor.Cyan;
            int c = DefaultTranslations == null ? 0 : DefaultTranslations.Count;
            Console.WriteLine("     Loading " + c + " translations");
            Console.ForegroundColor = ConsoleColor.White;

            translations = RocketTranslationManager.LoadTranslation(name, DefaultTranslations);
        }

        internal virtual void UnloadPlugin()
        {
            Unload();
            PluginManager.UnregisterCommands(GetType().Assembly);
            PluginManager.RemoveRocketPlayerComponents(GetType().Assembly);
            loaded = false;
        }

        public void OnEnable()
        {
            LoadPlugin();
        }

        public void OnDisable()
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