using Rocket.API;
using Rocket.API.Extensions;
using Rocket.Core;
using Rocket.Core.Assets;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using Rocket.Unturned.Events;
using Rocket.Unturned.Plugins;
using Rocket.Unturned.Serialisation;
using SDG.Unturned;
using Steamworks;
using System;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;

namespace Rocket.Unturned
{
    public class U : MonoBehaviour, IRocketImplementation
    {
        public static U Instance;
        public static R Rocket;

        public static XMLFileAsset<UnturnedSettings> Settings;
        public static XMLFileAsset<UnturnedTranslations> Translations;

        public IRocketImplementationEvents ImplementationEvents { get { return Events; } }
        public static ImplementationEvents Events;

        public static string Translate(string translationKey, params object[] placeholder)
        {
            return Translate(translationKey, placeholder);
        }

#if LINUX
        public Console Console;
#endif

        internal static void Splash()
        {
#if LINUX
            Console = new GameObject().AddComponent<RocketConsole>();
#endif
            System.Console.Clear();
            System.Console.ForegroundColor = ConsoleColor.Cyan;
            System.Console.WriteLine("Rocket Unturned v" + Assembly.GetExecutingAssembly().GetName().Version.ToString() + " for Unturned v" + Steam.Version + "\n");

            Steam.OnServerHosted += () =>
            {
                Launch();
            };
        }

        internal static void Launch()
        {
            GameObject rocketGameObject = new GameObject("Rocket");
            DontDestroyOnLoad(rocketGameObject);
            Rocket = rocketGameObject.TryAddComponent<Core.R>();
            Instance = rocketGameObject.TryAddComponent<U>();
        }


        private void Awake()
        {
            Environment.Initialize();
            Settings = new XMLFileAsset<UnturnedSettings>(Environment.SettingsFile);
            Translations = new XMLFileAsset<UnturnedTranslations>(string.Format(Environment.TranslationFile, Core.R.Settings.Instance.LanguageCode));
            Events = gameObject.TryAddComponent<ImplementationEvents>();

            gameObject.TryAddComponent<EffectManager>();

            RocketPlugin.OnPluginLoading += (IRocketPlugin plugin, ref bool cancelLoading) =>
            {
                try
                {
                    plugin.TryAddComponent<PluginUnturnedPlayerComponentManager>();
                    plugin.TryAddComponent<PluginCommandManager>();
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex, "Failed to load plugin "+ plugin.Name + ".");
                    cancelLoading = true;
                }
            };

            RocketPlugin.OnPluginUnloading += (IRocketPlugin plugin) =>
            {
                plugin.TryRemoveComponent<PluginUnturnedPlayerComponentManager>();
                plugin.TryRemoveComponent<PluginCommandManager>();
            };
        }

        public void Start()
        {
            //SteamGameServer.SetKeyValue("rocket", Assembly.GetExecutingAssembly().GetName().Version.ToString());
            //SteamGameServer.SetKeyValue("rocketplugins", String.Join(",", RocketPluginManager.GetPluginNames()));
            //SteamGameServer.SetBotPlayerCount(1);
        }

        public void Reload()
        {
            Translations.Reload();
            Settings.Reload();
        }

        public void Shutdown()
        {
            Steam.shutdown();
        }

        public void Execute(IRocketPlayer player, string command)
        {
            Commander.execute(new CSteamID(ulong.Parse(command)), command);
        }

        public string InstanceId
        {
            get { return Steam.InstanceName; }
        }
    }
}
