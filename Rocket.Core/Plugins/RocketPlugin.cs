using Rocket.API;
using Rocket.API.Collections;
using Rocket.API.Extensions;
using Rocket.Core.Assets;
using Rocket.Core.Extensions;
using Rocket.Core.Logging;
using System;
using System.Linq;
using UnityEngine;

namespace Rocket.Core.Plugins
{
    public class RocketPlugin<TConfiguration> : RocketPlugin, IRocketPlugin<TConfiguration> where TConfiguration : class
    {
        private IAsset<TConfiguration> configuration;
        public IAsset<TConfiguration> Configuration { get { return configuration; } }

        private string homeDirectory;
        public string HomeDirectory { get { return HomeDirectory; } }

        internal override void LoadPlugin()
        {
            if (Core.R.Settings.Instance.WebConfigurations.Enabled)
            {
                string url = string.Format(Environment.WebConfigurationTemplate, Core.R.Settings.Instance.WebConfigurations.Url, Name, R.Implementation.InstanceId);
                configuration = new WebXMLFileAsset<TConfiguration>(url);
            }
            else
            {
                configuration = new XMLFileAsset<TConfiguration>(string.Format(Core.Environment.PluginConfigurationFileTemplate, Name));
            }
            base.LoadPlugin();
        }
    }

    public class RocketPlugin : MonoBehaviour, IRocketPlugin
    {
        public delegate void PluginUnloading(IRocketPlugin plugin);
        public static event PluginUnloading OnPluginUnloading;

        public delegate void PluginLoading(IRocketPlugin plugin, ref bool cancelLoading);
        public static event PluginLoading OnPluginLoading;

        private IAsset<TranslationList> translations;
        public IAsset<TranslationList> Translations { get { return translations; } }

        private PluginState state = PluginState.Unloaded;
        public PluginState State
        {
            get
            {
                return state;
            }
        }

        public string Name
        {
            get
            {
                return GetType().Assembly.GetName().Name;
            }
        }

        public void ForceLoad()
        {
            LoadPlugin();
        }

        public void ForceUnload()
        {
            UnloadPlugin();
        }

        internal virtual void LoadPlugin()
        {
            DontDestroyOnLoad(transform.gameObject);

            try
            {
                Load();
            }
            catch (Exception ex)
            {
                Logger.LogError("Failed to load " + Name + ", unloading now... :" + ex.ToString());
                try
                {
                    UnloadPlugin(PluginState.Failure);
                    return;
                }
                catch (Exception ex1)
                {
                    Logger.LogError("Failed to unload " + Name + ":" + ex1.ToString());
                }
            }

            bool cancelLoading = false;
            if (OnPluginLoading != null)
            {
                foreach (var handler in OnPluginLoading.GetInvocationList().Cast<PluginLoading>())
                {
                    try
                    {
                        handler(this, ref cancelLoading);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogException(ex);
                    }
                    if (cancelLoading) {
                        try
                        {
                            UnloadPlugin(PluginState.Cancelled);
                            return;
                        }
                        catch (Exception ex1)
                        {
                            Logger.LogError("Failed to unload " + Name + ":" + ex1.ToString());
                        }
                    }
                }
            }
            state = PluginState.Loaded;
        }

        internal virtual void UnloadPlugin(PluginState state = PluginState.Unloaded)
        {
            OnPluginUnloading.TryInvoke(this);
            Unload();
            this.state = state;
        }

        private void OnEnable()
        {
            LoadPlugin();
        }

        private void OnDisable()
        {
            UnloadPlugin();
        }

        protected virtual void Load()
        {

        }


        protected virtual void Unload()
        {

        }

        public T TryAddComponent<T>() where T : Component
        {
            return gameObject.TryAddComponent<T>();
        }

        public void TryRemoveComponent<T>() where T : Component
        {
            gameObject.TryRemoveComponent<T>();
        }
    }
}