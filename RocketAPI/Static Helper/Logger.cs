using System;
using System.IO;
using UnityEngine;

namespace Rocket
{
    public class Logger : MonoBehaviour
    {
        private static string lastAssembly = "";
        /// <summary>
        /// Log an message to console
        /// </summary>
        /// <param name="message"></param>
        public static void Log(string message)
        {
            string assembly = System.Reflection.Assembly.GetCallingAssembly().GetName().Name;
            if (assembly == typeof(Logger).Assembly.GetName().Name || assembly == lastAssembly)
            {
                assembly = "";
            }
            else {
                assembly ="\n"+ assembly + " >> ";
            }
            lastAssembly = assembly;
            message = assembly + message;
            logToFile(message);
            Debug.Log(message);
        }
        /// <summary>
        /// Log a warning message to console
        /// </summary>
        /// <param name="message"></param>
        internal static void LogWarning(string message)
        {
            logToFile(message);
            Debug.LogWarning(message);
        }
        /// <summary>
        /// Log an error message to console
        /// </summary>
        /// <param name="message"></param>
        internal static void LogError(string message)
        {
            logToFile(message);
            Debug.LogError(message);
        }

        private static void logToFile(string message)
        {
            if (String.IsNullOrEmpty(RocketAPI.HomeFolder)) return;
            StreamWriter streamWriter = new StreamWriter(RocketAPI.HomeFolder + "Rocket.log", true);
            streamWriter.WriteLine("[" + DateTime.Now + "] " + message);
            streamWriter.Close();
        }
        /// <summary>
        /// Log Exception object stracktrace to console
        /// </summary>
        /// <param name="ex"></param>
        public static void LogException(Exception ex)
        {
            string assembly = System.Reflection.Assembly.GetCallingAssembly().GetName().Name;
            if (assembly == typeof(Logger).Assembly.GetName().Name || assembly == lastAssembly)
            {
                assembly = "";
            }
            else
            {
                assembly = "\n" + assembly + " >> ";
            }
            lastAssembly = assembly;
            logToFile(ex.ToString());
            System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
            Debug.LogError(assembly + "Error in " + stackTrace.GetFrame(1).GetMethod().Name + ": " + ex);
        }
    }
}
