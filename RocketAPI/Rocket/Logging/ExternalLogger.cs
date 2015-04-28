using Rocket.Rcon;
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Rocket.Logging
{
    public static partial class Logger
    {

        public static void ExternalLog(object msg)
        {
            ProcessLog(ELogType.Info, msg.ToString());
        }

        public static void ExternalLog(object msg, object context)
        {
            ProcessLog(ELogType.Info, msg.ToString(), context);
        }

        public static void ExternalLogError(object msg)
        {
            ProcessLog(ELogType.Error, msg.ToString());
        }

        public static void ExternalLogError(object msg, object context)
        {
            ProcessLog(ELogType.Error,  msg.ToString(), context);
        }

        public static void ExternalLogException(Exception ex)
        {
            ProcessLog(ELogType.Exception, ex.ToString());
        }

        public static void ExternalLogException(Exception ex, object context)
        {
            ProcessLog(ELogType.Exception, ex.ToString(), context);
        }

        public static void ExternalLogWarning(object msg)
        {
            ProcessLog(ELogType.Warning, msg.ToString());
        }

        public static void ExternalLogWarning(object msg, object context)
        {
            ProcessLog(ELogType.Warning, msg.ToString(), context);
        }

        private static void ProcessLog(ELogType type, string message, object context = null)
        {
            if (String.IsNullOrEmpty(RocketSettings.HomeFolder) || !Directory.Exists(RocketSettings.HomeFolder + "Logs/")) return;
            StreamWriter streamWriter = File.AppendText(RocketSettings.HomeFolder + "Logs/" + "Rocket.log");
            streamWriter.WriteLine("[" + DateTime.Now + "] " + message);
            streamWriter.Close();
            if(RocketSettings.EnableRcon)
            RocketRconServer.broadcast(message);
        }
    }
}