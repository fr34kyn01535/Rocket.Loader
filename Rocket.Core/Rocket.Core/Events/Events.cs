using Rocket.Core.Logging;

namespace Rocket.Core.Events
{
    public static class RocketEvents
    {
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
