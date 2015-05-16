using System;
using System.ComponentModel;

namespace Rocket.Unturned.Logging
{
    public class Logger
    {
        public static void Log(string message)
        {
            Core.Logging.Logger.Log(message);
        }

        public static void LogWarning(string message)
        {
            Core.Logging.Logger.LogWarning(message);
        }

        public static void LogError(string message)
        {
            Core.Logging.Logger.LogError(message);
        }

        public static void LogException(Exception ex)
        {
            Core.Logging.Logger.LogException(ex);
        }


        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void ExternalLog(string message, ConsoleColor color)
        {
            Core.Logging.Logger.ExternalLog(message,color);
        }
    }
}
