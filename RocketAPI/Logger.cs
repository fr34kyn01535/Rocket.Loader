using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Rocket.RocketAPI
{
    public static class Logger
    {
        /// <summary>
        /// Log an message to console
        /// </summary>
        /// <param name="message"></param>
        public static void Log(string message)
        {
            logToFile(message);
            Debug.Log(message);
        }
        /// <summary>
        /// Log a warning message to console
        /// </summary>
        /// <param name="message"></param>
        public static void LogWarning(string message)
        {
            logToFile(message);
            Debug.LogWarning(message);
        }
        /// <summary>
        /// Log an error message to console
        /// </summary>
        /// <param name="message"></param>
        public static void LogError(string message)
        {
            logToFile(message);
            Debug.LogError(message);
        }

        private static void logToFile(string message)
        {
            StreamWriter streamWriter = new StreamWriter(Bootstrap.HomeFolder + "Rocket.log", true);
            streamWriter.WriteLine(message);
            streamWriter.Close();
        }
        /// <summary>
        /// Log Exception object stracktrace to console
        /// </summary>
        /// <param name="ex"></param>
        public static void LogException(Exception ex)
        {
            logToFile(ex.ToString());
            System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
            Debug.LogError("Error in " + stackTrace.GetFrame(1).GetMethod().Name+": "+ex);
        }
    }
}
