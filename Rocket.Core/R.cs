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
using Rocket.API.Collections;

namespace Rocket.Core
{
    public class R : MonoBehaviour
    {
        public static R Instance;
        public static IRocketImplementation Implementation;

        public static XMLFileAsset<RocketSettings> Settings;
        public static XMLFileAsset<TranslationList> Translation;
        public static RocketPermissionsManager Permissions;
        public static RocketPluginManager Plugins;

        private static readonly TranslationList defaultTranslations = new TranslationList(){
                {"rocket_join_public","{0} connected to the server" },
                {"rocket_leave_public","{0} disconnected from the server"},
                {"rocket_restart_warning_public","This server will be restarted in 30 seconds"}
        };

        private void Awake()
        {
            Instance = this;
            Environment.Initialize();
            Implementation = (IRocketImplementation)GetComponent(typeof(IRocketImplementation));

            Settings = new XMLFileAsset<RocketSettings>(Environment.SettingsFile);
            Translation = new XMLFileAsset<TranslationList>(String.Format(Environment.TranslationFile, Settings.Instance.LanguageCode), new Type[]{ typeof(TranslationList), typeof(TranslationListEntry) }, defaultTranslations);
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