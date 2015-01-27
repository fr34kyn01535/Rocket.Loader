using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Rocket.RocketAPI
{
    public static class Logger
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
                assembly = assembly + " >> ";
            }
            lastAssembly = assembly;
            message = assembly + message;
            logToFile(message);
            Debug.Log(message);
        }

        public static void Log(object o)
        {
            string message = var_dump(o);
            logToFile(message);
            Debug.LogWarning(message);
        }

        internal static string var_dump(object obj, int recursion = 0)
        {
            StringBuilder result = new StringBuilder();

            if (recursion < 5)
            {
                Type t = obj.GetType();
                PropertyInfo[] properties = t.GetProperties();

                foreach (PropertyInfo property in properties)
                {
                    try
                    {
                        object value = property.GetValue(obj, null);
                        string indent = String.Empty;
                        string spaces = "|   ";
                        string trail = "|...";

                        if (recursion > 0)
                        {
                            indent = new StringBuilder(trail).Insert(0, spaces, recursion - 1).ToString();
                        }

                        if (value != null)
                        {
                            string displayValue = value.ToString();
                            if (value is string) displayValue = String.Concat('"', displayValue, '"');
                            result.AppendFormat("{0}{1} = {2}\n", indent, property.Name, displayValue);

                            try
                            {
                                if (!(value is ICollection))
                                {
                                    result.Append(var_dump(value, recursion + 1));
                                }
                                else
                                {
                                    int elementCount = 0;
                                    foreach (object element in ((ICollection)value))
                                    {
                                        string elementName = String.Format("{0}[{1}]", property.Name, elementCount);
                                        indent = new StringBuilder(trail).Insert(0, spaces, recursion).ToString();
                                        result.AppendFormat("{0}{1} = {2}\n", indent, elementName, element.ToString());
                                        result.Append(var_dump(element, recursion + 2));
                                        elementCount++;
                                    }

                                    result.Append(var_dump(value, recursion + 1));
                                }
                            }
                            catch { }
                        }
                        else
                        {
                            result.AppendFormat("{0}{1} = {2}\n", indent, property.Name, "null");
                        }
                    }
                    catch
                    {
                        // Some properties will throw an exception on property.GetValue()
                        // I don't know exactly why this happens, so for now i will ignore them...
                    }
                }
            }

            return result.ToString();
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
            if (String.IsNullOrEmpty(RocketSettings.HomeFolder)) return;
            StreamWriter streamWriter = new StreamWriter(RocketSettings.HomeFolder + "Rocket.log", true);
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
