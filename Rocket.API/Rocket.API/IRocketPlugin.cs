using System.Collections.Generic;

namespace Rocket.API
{

    public interface IRocketPlugin<TConfiguration> : IRocketPlugin
    {
        TConfiguration Configuration { get;}

        string HomeDirectory { get; }

        void ReloadConfiguration();
    }

    public interface IRocketPlugin
    {
        bool Loaded { get; }

        Dictionary<string, string> Translations { get; }

        Dictionary<string, string> DefaultTranslations { get; }

        void ReloadTranslation();

        string Translate(string translationKey, params object[] placeholder);
    }
}