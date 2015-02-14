using SDG;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace VersionInfo
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                switch (args[0]) { 
                    case "rocket":
                        Console.Write(getRocketVersion());
                        Environment.Exit(0);
                        break;
                    case "unturned":
                        Console.Write(getUnturnedVersion());
                        Environment.Exit(0);
                        break;
                }
            }
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
