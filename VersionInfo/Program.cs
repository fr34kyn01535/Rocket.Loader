using SDG;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;

namespace VersionInfo
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 3) Environment.Exit(1);
            WebClient wc = new WebClient();
            wc.DownloadData(String.Format("http://api.rocket.foundation/jenkins/version.php?build={0}&buildid={1}&rocketversion={2}&unturnedversion={3}", args[1], args[2], getRocketVersion(), getUnturnedVersion()));
        }

        private static string getRocketVersion()
        {
            Assembly unturned = Assembly.Load(File.ReadAllBytes("RocketAPI.dll"));
            return unturned.GetName().Version.ToString();
        }

        private static string getUnturnedVersion()
        {
            Type t = typeof(Steam);
            FieldInfo field = t.GetFields().Where(f => f.IsStatic == true && f.FieldType == typeof(string)).FirstOrDefault();
            return (string)field.GetValue(null);
        }
    }
}
