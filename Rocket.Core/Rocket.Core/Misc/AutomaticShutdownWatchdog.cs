using Rocket.Core.Events;
using Rocket.Core.Logging;
using Rocket.Core.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Rocket.Core.Misc
{
    internal class AutomaticShutdownWatchdog : MonoBehaviour
    {
        private void FixedUpdate()
        {
            if (started)
            {
                checkTimerRestart();
                checkTimerSave();
            }
        }

        private DateTime? shutdownTime = null;
        private DateTime lastSaveTime;
        private bool shutdown = false;
        public static AutomaticShutdownWatchdog Instance;
        private bool started = false;

        private void Start()
        {
#if DEBUG
            Logger.Log("AutomaticShutdownWatchdog > Start");
#endif
            DontDestroyOnLoad(transform.gameObject);
            Instance = this;
            if (RocketSettingsManager.Settings.AutomaticShutdown.Enabled)
            {
                shutdownTime = RocketBootstrap.Started.ToUniversalTime().AddSeconds(RocketSettingsManager.Settings.AutomaticShutdown.Interval);
                Logger.Log("The server will automaticly shutdown in " + RocketSettingsManager.Settings.AutomaticShutdown.Interval + " seconds (" + shutdownTime.ToString() + " UTC)");
            }
            lastSaveTime = DateTime.UtcNow;
            started = true;
        }

        private void checkTimerSave()
        {
            try
            {
                if (RocketSettingsManager.Settings.AutomaticSaveInterval > 0 && (DateTime.UtcNow - lastSaveTime).TotalSeconds > RocketSettingsManager.Settings.AutomaticSaveInterval)
                {
                    RocketEvents.triggerOnRocketSave();
                    Logger.Log("Server saved.");
                    lastSaveTime = DateTime.UtcNow;
                }
            }
            catch (Exception er)
            {
                Logger.LogException(er);
            }
        }

        private void checkTimerRestart()
        {
            try
            {
                if (shutdownTime != null)
                {
                    if ((shutdownTime.Value - DateTime.UtcNow).TotalSeconds < 0 && !shutdown)
                    {
                        shutdown = true;
                        RocketEvents.triggerOnRocketAutomaticShutdown();
                    }
                }
            }
            catch (Exception er)
            {
                Logger.LogException(er);
            }
        }
        private static string combine(string path1, string path2)
        {
            path1 = path1.TrimEnd(new char[] { '/' });
            path2 = path2.TrimStart(new char[] { '/' });
            return string.Format("{0}/{1}", path1, path2);
        }

    }
}
