using Rocket.RCON;
using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Rocket.Logging
{
    public static partial class Logger
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void ExternalLog(string message, ConsoleColor color)
        {
            ELogType severity;
            switch (color)
            {
                case ConsoleColor.Red:
                    severity = ELogType.Error;
                    break;
                case ConsoleColor.Yellow:
                    severity = ELogType.Warning;
                    break;
                default:
                    severity = ELogType.Info;
                    break;
            }
            ProcessLog(severity, message);
        }
    }
}