﻿using Rocket.API;
using Rocket.Core.Logging;
using Rocket.Core.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using System.Linq;
using Rocket.Core.Extensions;

namespace Rocket.Core.Plugins
{
    public sealed class RocketPluginManager : MonoBehaviour
    {
        public delegate void PluginsLoaded();
        public event PluginsLoaded OnPluginsLoaded;

        private static List<Assembly> pluginAssemblies;
        private static List<GameObject> plugins = new List<GameObject>();
        internal static List<IRocketPlugin> Plugins { get { return plugins.Select(g => g.GetComponent<IRocketPlugin>()).Where(p => p != null).ToList<IRocketPlugin>(); } }
        private Dictionary<string, string> libraries = new Dictionary<string, string>();

        public List<IRocketPlugin> GetPlugins()
        {
            return Plugins;
        }

        private void Awake() {
            AppDomain.CurrentDomain.AssemblyResolve += delegate (object sender, ResolveEventArgs args)
            {
                string file;
                if (libraries.TryGetValue(args.Name, out file))
                {
                    return Assembly.LoadFile(file);
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
            libraries = GetAssembliesFromDirectory(Environment.LibrariesDirectory);
            pluginAssemblies = LoadAssembliesFromDirectory(Environment.PluginsDirectory);
            List<Type> pluginImplemenations = RocketHelper.GetTypesFromInterface(pluginAssemblies, "IRocketPlugin");
            foreach (Type pluginType in pluginImplemenations)
            {
                GameObject plugin = new GameObject(pluginType.Name, pluginType);
                DontDestroyOnLoad(plugin);
                plugins.Add(plugin);
            }
            OnPluginsLoaded.TryInvoke();
        }

        private void unloadPlugins() {
            for(int i = plugins.Count; i > 0; i--)
            {
                Destroy(plugins[i-1]);
            }
            plugins.Clear();
        }

        internal void Reload()
        {
            unloadPlugins();
            loadPlugins();
        }

        public static Dictionary<string, string> GetAssembliesFromDirectory(string directory, string extension = "*.dll")
        {
            Dictionary<string, string> l = new Dictionary<string, string>();
            IEnumerable<FileInfo> libraries = new DirectoryInfo(directory).GetFiles(extension, SearchOption.AllDirectories);
            foreach (FileInfo library in libraries)
            {
                try
                {
                    AssemblyName name = AssemblyName.GetAssemblyName(library.FullName);
                    l.Add(name.FullName, library.FullName);
                }
                catch { }
            }
            return l;
        }

        public static List<Assembly> LoadAssembliesFromDirectory(string directory, string extension = "*.dll")
        {
            List<Assembly> assemblies = new List<Assembly>();
            IEnumerable<FileInfo> pluginsLibraries = new DirectoryInfo(directory).GetFiles(extension, SearchOption.TopDirectoryOnly);

            foreach (FileInfo library in pluginsLibraries)
            {
                try
                {
                    Assembly assembly = Assembly.LoadFile(library.FullName);

                    if (RocketHelper.GetTypesFromInterface(assembly, "IRocketPlugin").Count == 1)
                    {
                        assemblies.Add(assembly);
                    }
                    else
                    {
                        Logger.LogError("Invalid or outdated plugin assembly: " + assembly.GetName().Name);
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "Could not load plugin assembly: " + library.Name);
                }
            }
            return assemblies;
        }
    }
}