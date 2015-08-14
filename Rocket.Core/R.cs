﻿using System;
using UnityEngine;
using Rocket.Core.RCON;
using Rocket.API;
using Rocket.Core.Plugins;
using Rocket.Core.Permissions;
using Rocket.Core.Utils;
using Rocket.Core.Assets;
using Rocket.API.Extensions;
using Rocket.Core.Serialization;
using Rocket.API.Collections;
using Rocket.Core.Extensions;
using Rocket.Core.Logging;

namespace Rocket.Core
{
    public class R : MonoBehaviour
    {
        public delegate void RockedInitialized();
        public static event RockedInitialized OnRockedInitialized;

        public static R Instance;
        public static IRocketImplementation Implementation;

        public static XMLFileAsset<RocketSettings> Settings = null;
        public static XMLFileAsset<TranslationList> Translation = null;
        public static RocketPermissionsManager Permissions = null;
        public static RocketPluginManager Plugins = null;

        private static readonly TranslationList defaultTranslations = new TranslationList(){
                {"rocket_join_public","{0} connected to the server" },
                {"rocket_leave_public","{0} disconnected from the server"},
                {"rocket_restart_warning_public","This server will be restarted in 30 seconds"}
        };

        private void Awake()
        {
            Instance = this;
            Implementation = (IRocketImplementation)GetComponent(typeof(IRocketImplementation));

            #if DEBUG
                gameObject.TryAddComponent<Debugger>();
            #else
                Initialize();
            #endif
        }

        internal void Initialize()
        {   
            Environment.Initialize();
            try
            {
                Implementation.OnRocketImplementationInitialized += () =>
                {
                    gameObject.TryAddComponent<RocketDispatcher>();
                    gameObject.TryAddComponent<AutomaticShutdownWatchdog>();
                    gameObject.TryAddComponent<RCONServer>();
                };

                Settings = new XMLFileAsset<RocketSettings>(Environment.SettingsFile);
                Translation = new XMLFileAsset<TranslationList>(String.Format(Environment.TranslationFile, Settings.Instance.LanguageCode), new Type[] { typeof(TranslationList), typeof(TranslationListEntry) }, defaultTranslations);
                Permissions = gameObject.TryAddComponent<RocketPermissionsManager>();
                Plugins = gameObject.TryAddComponent<RocketPluginManager>();

                if (Settings.Instance.MaxFrames < 10 && Settings.Instance.MaxFrames != -1) Settings.Instance.MaxFrames = 10;
                Application.targetFrameRate = Settings.Instance.MaxFrames;

                OnRockedInitialized.TryInvoke();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
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