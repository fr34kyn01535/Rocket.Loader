﻿using System;
using System.Collections;
using System.Reflection;
using System.Text;

namespace Rocket.Core.Logging
{
    public partial class Logger
    {
        private static string lastAssembly = "";

        public static void Log(string message)
        {
            System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();

            string assembly = "";
            if (stackTrace.FrameCount != 0)
                assembly = stackTrace.GetFrame(1).GetMethod().DeclaringType.Assembly.GetName().Name;

            if (assembly == "Rocket.Unturned" && stackTrace.FrameCount > 1)
            {
                assembly = stackTrace.GetFrame(2).GetMethod().DeclaringType.Assembly.GetName().Name;
            }

            if (assembly == "" || assembly == typeof(Logger).Assembly.GetName().Name || assembly == lastAssembly || assembly == "Rocket.Unturned")
            {
                assembly = "";
            }
            else
            {
                assembly = assembly + " >> ";
            }

            lastAssembly = assembly;
            message = assembly + message;
            ProcessInternalLog(ELogType.Info, message);
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

        public static void LogWarning(string message)
        {
            ProcessInternalLog(ELogType.Warning, message);
        }

        public static void LogError(string message)
        {
            ProcessInternalLog(ELogType.Error, message);
        }

        public static void Log(Exception ex)
        {
            LogException(ex);
        }

        public static void LogException(Exception ex)
        {
            System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
            string source = stackTrace.GetFrame(1).GetMethod().Name;
            string assembly = stackTrace.GetFrame(1).GetMethod().DeclaringType.Assembly.GetName().Name;
            if (assembly == "Rocket.Unturned" && stackTrace.FrameCount > 1)
            {
                source = stackTrace.GetFrame(2).GetMethod().Name;
                assembly = stackTrace.GetFrame(2).GetMethod().DeclaringType.Assembly.GetName().Name;
            }
            lastAssembly = assembly;
            ProcessInternalLog(ELogType.Exception, assembly + " >> Exception in " + source + ": " + ex);
        }

        private static void ProcessInternalLog(ELogType type, string message)
        {
            if (type == ELogType.Error || type == ELogType.Exception)
            {

               // Debug.LogError(message);
                writeToConsole(message, ConsoleColor.Red);
            }
            else if (type == ELogType.Warning)
            {
              //  Debug.LogWarning(message);
                writeToConsole(message, ConsoleColor.Yellow);
            }
            else
            {
                //Debug.Log(message);
                writeToConsole(message, ConsoleColor.White);
            }
            ProcessLog(type,message);
        }

        internal static void logRCON(string message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            string m = "RocketRCON >> " + message;
            ProcessLog(ELogType.Info, message);
            Console.WriteLine(m);
        }

        private static void writeToConsole(string message,ConsoleColor color){
            Console.ForegroundColor = color;
            Console.WriteLine(message);
        }


        private static void ProcessLog(ELogType type, string message)
        {
            AsyncLoggerQueue.Current.Enqueue(new LogEntry() { Severity = type, Message = message });
        }

    }
}