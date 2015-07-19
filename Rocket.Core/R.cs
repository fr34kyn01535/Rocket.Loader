using System;
using UnityEngine;
using Rocket.Core.RCON;
using Rocket.API;
using Rocket.Core.Plugins;
using Rocket.Core.Permissions;
using Rocket.Core.Misc;
using Rocket.Core.Assets;
using Rocket.API.Extensions;
using Rocket.Core.Serialization;

namespace Rocket.Core
{
    public class R : MonoBehaviour
    {
        public static R Instance;
        public static IRocketImplementation Implementation;

        public static XMLFileAsset<RocketSettings> Settings;
        public static XMLFileAsset<RocketTranslations> Translation;
        public static RocketPermissionsManager Permissions;
        public static RocketPluginManager Plugins;

        private void Awake()
        {
            Environment.Initialize();
            Implementation = (IRocketImplementation)GetComponent(typeof(IRocketImplementation));

            Settings = new XMLFileAsset<RocketSettings>(Environment.SettingsFile);
            Translation = new XMLFileAsset<RocketTranslations>(String.Format(Environment.TranslationFile, Settings.Instance.LanguageCode));
            Permissions = gameObject.TryAddComponent<RocketPermissionsManager>();
            Plugins = gameObject.TryAddComponent<RocketPluginManager>();
        }

        private void Start()
        {
            gameObject.TryAddComponent<AutomaticShutdownWatchdog>();
            gameObject.TryAddComponent<RCONServer>();
        }

        public static void Reload()
        {
            Settings.Reload();
            Translation.Reload();
            Permissions.Reload();
            Plugins.Reload();
            Implementation.Reload();
        }
    }
}