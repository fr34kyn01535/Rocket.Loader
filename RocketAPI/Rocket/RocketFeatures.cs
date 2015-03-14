using Rocket.Components;
using Rocket.Logging;
using Rocket.RocketAPI;
using SDG;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Rocket
{
    internal class RocketFeatures : RocketManagerComponent
    {
        private void FixedUpdate()
        {
            checkTimerRestart();
        }

        private DateTime? shutdownTime = null;
        private bool notificationShown = false;
        private bool shutdown = false;
        public static RocketFeatures Instance;

        private new void Awake()
        {
#if DEBUG
            Logger.Log("Awake RocketFeatures");
#endif
            base.Awake();
            Instance = this;
            if (RocketSettings.AutomaticShutdownInterval != 0)
            {
                shutdownTime = Bootstrap.Started.ToUniversalTime().AddSeconds(RocketSettings.AutomaticShutdownInterval);
                Logger.Log("The server will automaticly shutdown in " + RocketSettings.AutomaticShutdownInterval + " seconds (" + shutdownTime.ToString()+")");
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
                        if (RocketSettings.AutomaticShutdownClearLevel && Directory.Exists(combine(RocketSettings.HomeFolder, "../Level/")))
                        {
                            Logger.Log("Deleting Level...");
                            Directory.Delete(combine(RocketSettings.HomeFolder, "../Level/"), true);
                        }
                        if (RocketSettings.AutomaticShutdownClearPlayers && Directory.Exists(combine(RocketSettings.HomeFolder, "../Players/")))
                        {
                            Logger.Log("Deleting Players...");
                            Directory.Delete(combine(RocketSettings.HomeFolder, "../Players/"), true);
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
