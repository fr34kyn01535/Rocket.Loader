using Rocket.API;
using Rocket.Core.Logging;
using Rocket.Core.Misc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using Rocket.Core.Extensions;

namespace Rocket.Core.Plugins
{
    public sealed class RocketPluginManager : MonoBehaviour
    {
        public delegate void PluginsLoaded();
        public event PluginsLoaded OnPluginsLoaded;

        private static List<Assembly> pluginAssemblies;
        private static List<IRocketPlugin> plugins = new List<IRocketPlugin>();
        public static List<IRocketPlugin> Plugins { get { return plugins; } }
        private Dictionary<string, string> libraries = new Dictionary<string, string>();

        private void Awake() {
            AppDomain.CurrentDomain.AssemblyResolve += delegate (object sender, ResolveEventArgs args)
            {
                string file;
                if (libraries.TryGetValue(args.Name, out file))
                {
                    return Assembly.Load(File.ReadAllBytes(file));
                }
                else
                {
                    Logger.LogError("Could not find dependency: " + args.Name);
                }
                return null;
            };
        }

        private void Start()
        {
            loadPlugins();
        }

        private void loadPlugins()
        {
            libraries = RocketHelper.GetAssembliesFromDirectory(Environment.LibrariesDirectory);
            pluginAssemblies = RocketHelper.LoadAssembliesFromDirectory(Environment.PluginsDirectory);
            List<Type> pluginImplemenations = RocketHelper.GetTypesFromInterface(pluginAssemblies, "IRocketPlugin");
            foreach (Type plugin in pluginImplemenations)
            {
                plugins.Add((IRocketPlugin)R.Instance.gameObject.AddComponent(plugin));
            }
            OnPluginsLoaded.TryInvoke();
        }

        private void unloadPlugins() {
            for(int i = plugins.Count; i > 0; i--)
            {
                Destroy((Component)plugins[i]);
            }
            plugins.Clear();
        }

        internal void Reload()
        {
            unloadPlugins();
            loadPlugins();
        }
    }
}