using Rocket.Core.Logging;
using Rocket.Core.Permissions;
using Rocket.Core.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Rocket.Core.Misc
{
    public class SettingsWatcher : MonoBehaviour
    {
        public void Start()
        {
#if DEBUG
            Logger.Log("SettingsWatcher > Awake");
#endif
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = RocketBootstrap.Implementation.ConfigurationFolder;
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Filter = "*.config.xml";
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.EnableRaisingEvents = true;
        }

        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            string name = Path.GetFileName(e.FullPath);
#if DEBUG
            Logger.Log("SettingsWatcher > OnChanged > "+name);
#endif
            if (name == RocketBootstrap.PermissionFile)
            {
                RocketPermissionsManager.Reload(false);
            }
            if (name == RocketBootstrap.SettingsFile)
            {
                RocketSettingsManager.Reload(false);
            }
        }
    }
}
