using Rocket.RocketAPI.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Rocket.RocketAPI
{
    public class RocketPlugin<T> : RocketPlugin
    {
        public static T Configuration;

        private new void Awake()
        {
            try
            {
                Configuration = RocketConfiguration.LoadConfiguration<T>();
            }
            catch (Exception ex)
            {
                Logger.LogError("Failed to load configuration: "+ex.ToString());
            }
        }
    }

    public class RocketPlugin : RocketManagerComponent
    {
        public void Start()
        {
            Load();
        }
        protected virtual void Load() { }
    }
}
