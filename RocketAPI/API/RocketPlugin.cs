using Rocket.RocketAPI.Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Rocket.RocketAPI
{
    public class RocketPlugin<T> : RocketPlugin
    {
        public static T Configuration;
        public static string HomeDirectory;

        private new void Awake()
        {
            HomeDirectory = RocketSettings.HomeFolder + "Plugins/" + (typeof(T).Assembly.GetName().Name);
            if (!Directory.Exists(HomeDirectory)) Directory.CreateDirectory(HomeDirectory);

            try
            {
                Configuration = RocketConfigurationHelper.LoadConfiguration<T>();
            }
            catch (Exception ex)
            {
                Logger.LogError("Failed to load configuration: "+ex.ToString());
            }
            Loaded = true;
        }
    }

    public class RocketPlugin : RocketManagerComponent
    {
        public static bool Loaded = false;
        public void Start()
        {
            Load();
        }
        private new void Awake()
        {
            Loaded = true;
        }
        protected virtual void Load() { }
    }
}
