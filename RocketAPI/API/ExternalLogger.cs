using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Rocket.RocketAPI
{
    public static partial class Logger
    {

        public static void ExternalLog(object msg)
        {

            if(msg != null)
                processLog("[Info] " + msg.ToString());

        }

        public static void ExternalLog(object msg, object context)
        {

            if (msg != null)
                processLog("[Info] " + msg.ToString());

            if (context != null)
                processLog("[Info] [Context] " + context.ToString());

        }

        public static void ExternalLogError(object msg)
        {

            if (msg != null)
                processLog("[Error] " + msg.ToString());

        }

        public static void ExternalLogError(object msg, object context)
        {

            if (msg != null)
                processLog("[Error] " + msg.ToString());

            if (context != null)
                processLog("[Error] [Context] " + msg.ToString());

        }

        public static void ExternalLogException(Exception ex)
        {

            if (ex != null)
                processLog("[Exception] " + ex);

        }

        public static void ExternalLogException(Exception ex, object context)
        {

            if (ex != null)
                processLog("[Exception] " + ex);

            if (context != null)
                processLog("[Exception] [Context] " + context.ToString());

        }

        public static void ExternalLogWarning(object msg)
        {

            if (msg != null)
                processLog("[Warning] " + msg.ToString());

        }

        public static void ExternalLogWarning(object msg, object context)
        {

            if (msg != null)
                processLog("[Warning] " + msg.ToString());

            if (context != null)
                processLog("[Warning] [Context] " + context.ToString());

        }

    }
}