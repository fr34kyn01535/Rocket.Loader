using Rocket.Core.Logging;
using System;
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
            }
        }

        private DateTime? shutdownTime = null;
        private DateTime lastSaveTime;
        private bool shutdown = false;
        public static AutomaticShutdownWatchdog Instance;
        private bool started = false;
        private DateTime startedTime = DateTime.Now;

        private void Start()
        {
            Instance = this;
            if (R.Settings.Instance.AutomaticShutdown.Enabled)
            {
                shutdownTime = startedTime.ToUniversalTime().AddSeconds(R.Settings.Instance.AutomaticShutdown.Interval);
                Logger.Log("The server will automaticly shutdown in " + R.Settings.Instance.AutomaticShutdown.Interval + " seconds (" + shutdownTime.ToString() + " UTC)");
            }
            lastSaveTime = DateTime.UtcNow;
            started = true;
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
                        R.Implementation.Shutdown();
                    }
                }
            }
            catch (Exception er)
            {
                Logger.LogException(er);
            }
        }
    }
}
