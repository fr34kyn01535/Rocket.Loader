using Rocket.Core.Logging;
using System;
using System.Linq;

namespace Rocket.Core.Events
{
    public static class RocketEvents
    {
        public static void TryTrigger<T>(MulticastDelegate theDelegate, params object[] args)
        {
            if (theDelegate == null) return;
            foreach (var handler in theDelegate.GetInvocationList().Cast<T>())
            {
                try
                {
                    theDelegate.DynamicInvoke(args);
                }
                catch (Exception ex)
                {
                    Logger.LogError("Error in Event "+theDelegate.GetType().Name+": "+ex.ToString());
                }
            }
        }

        public delegate void RocketSaved();
        public static event RocketSaved OnRocketSave;

        internal static void triggerOnRocketSave()
        {
            try
            {
                if (OnRocketSave != null) OnRocketSave();
            }
            catch (System.Exception ex)
            {
                Logger.Log(ex);
            }
        }

        public delegate void RocketAutomaticShutdown();
        public static event RocketAutomaticShutdown OnRocketAutomaticShutdown;

        internal static void triggerOnRocketAutomaticShutdown()
        {
            try
            {
                if (OnRocketAutomaticShutdown != null) OnRocketAutomaticShutdown();
            }
            catch (System.Exception ex)
            {
                Logger.Log(ex);
            }
        }


        public delegate void RocketCommandTriggered(string command,ref bool success);
        public static event RocketCommandTriggered OnRocketCommandTriggered;

        internal static bool triggerOnRocketCommandTriggered(string command)
        {
            try
            {
                bool success = true;
                if (OnRocketCommandTriggered != null) OnRocketCommandTriggered(command,ref success);
                return success;
            }
            catch (System.Exception ex)
            {
                Logger.Log(ex);
            }
            return false;
        }
    }
}
