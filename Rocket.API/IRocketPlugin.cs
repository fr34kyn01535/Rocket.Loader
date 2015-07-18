using Rocket.API.Collections;
using Rocket.Core.Assets;
using UnityEngine;

namespace Rocket.API
{
    public enum PluginState { Loaded, Unloaded, Failure, Cancelled };

    public interface IRocketPlugin<TConfiguration> : IRocketPlugin where TConfiguration : class
    {
        IAsset<TConfiguration> Configuration { get;}
        string HomeDirectory { get; }
    }

    public interface IRocketPlugin
    {
        string Name { get; }
        PluginState State { get; }
        IAsset<TranslationList> Translations { get; }
        T TryAddComponent<T>() where T : UnityEngine.Component;
        void TryRemoveComponent<T>() where T : UnityEngine.Component;
    }
}