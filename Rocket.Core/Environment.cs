using System;
using System.IO;

namespace Rocket.Core
{
    public static class Environment
    {
        public static void Initialize()
        {
            if (!Directory.Exists(PluginsDirectory)) Directory.CreateDirectory(PluginsDirectory);
            if (!Directory.Exists(LibrariesDirectory)) Directory.CreateDirectory(LibrariesDirectory);
            if (!Directory.Exists(LogsDirectory)) Directory.CreateDirectory(LogsDirectory);
            if (File.Exists(LogFile))
            {
                string ver = ((int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds).ToString();
                File.Move(LogFile, LogsDirectory + "Rocket." + ver + ".log");
            };
        }

        public static readonly string PluginsDirectory = "Plugins";
        public static readonly string LibrariesDirectory = "Libraries";
        public static readonly string LogsDirectory = "Logs";

        public static readonly string SettingsFile = "Rocket.config.xml";
        public static readonly string TranslationFile = "Rocket.{0}.translation.xml";
        public static readonly string LogFile = "Logs/Rocket.log";
        public static readonly string PermissionFile = "Permissions.config.xml";

        public static readonly string PluginTranslationFileTemplate = "Plugins/{0}/{0}.{1}.translation.xml";
        public static readonly string PluginConfigurationFileTemplate = "Plugins/{0}/{0}.configuration.xml";

        public static readonly string WebConfigurationTemplate = "{0}?configuration={1}&instance{2}";
    }
}
