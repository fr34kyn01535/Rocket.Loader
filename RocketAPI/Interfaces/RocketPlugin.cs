using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace Rocket.RocketAPI
{
    /// <summary>
    /// This is the interface for plugins
    /// </summary>
    public interface RocketPlugin
    {
        /// <summary>
        /// The name of the plugin author, the plugin name and version are retrieved from the AssemblyInfo.cs
        /// </summary>
        string Author { get; }

        /// <summary>
        /// This method will be called when this plugin is loaded
        /// </summary>
        void Load();

    }
}
