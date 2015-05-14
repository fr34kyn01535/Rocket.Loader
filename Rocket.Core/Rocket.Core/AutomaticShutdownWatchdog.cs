using Rocket.Core.Logging;
using SDG;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Rocket.Core
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

        private void Start() {
            started = true;
        }

        private DateTime? shutdownTime = null;
        private DateTime lastSaveTime;
        private bool notificationShown = false;
        private bool shutdown = false;
        public static AutomaticShutdownWatchdog Instance;
        private bool started = false;

        private new void Awake()
        {
#if DEBUG
            Logger.Log("Awake RocketFeatures");
#endif
            DontDestroyOnLoad(transform.gameObject);
            Instance = this;
            if (SettingsManager.AutoShutdownInterval != 0)
            {
                shutdownTime = Rocket.Started.ToUniversalTime().AddSeconds(SettingsManager.AutoShutdownInterval);
                Logger.Log("The server will automaticly shutdown in " + SettingsManager.AutoShutdownInterval + " seconds (" + shutdownTime.ToString()+" UTC)");
            }
            lastSaveTime = DateTime.UtcNow;
        }

        private void checkTimerSave()
        {
            try
            {
                if (SettingsManager.AutoSaveInterval > 0 && (DateTime.UtcNow - lastSaveTime).TotalSeconds > SettingsManager.AutoSaveInterval)
                {
                    SaveManager.save();
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
                    if ((shutdownTime.Value - DateTime.UtcNow).TotalSeconds < 30 && !notificationShown)
                    {
                        notificationShown = true;
                        RocketChatManager.Say(RocketTranslation.Translate("rocket_restart_warning_public"));
                    }
                    if ((shutdownTime.Value - DateTime.UtcNow).TotalSeconds < 0 && !shutdown)
                    {
                        shutdown = true;
                        if (SettingsManager.AutoShutdownClearLevel && Directory.Exists(combine(SettingsManager.HomeFolder, "../Level/")))
                        {
                            Logger.Log("Deleting Level...");
                            Directory.Delete(combine(SettingsManager.HomeFolder, "../Level/"), true);
                        }
                        if (SettingsManager.AutoShutdownClearPlayers && Directory.Exists(combine(SettingsManager.HomeFolder, "../Players/")))
                        {
                            Logger.Log("Deleting Players...");
                            Directory.Delete(combine(SettingsManager.HomeFolder, "../Players/"), true);
                        }
                        Logger.Log("Shutting down...");
                        Steam.shutdown();
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
