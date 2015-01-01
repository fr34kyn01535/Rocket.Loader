using SDG;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Rocket.RocketAPI.Interfaces
{
    /// <summary>
    /// This is the interface for commands
    /// </summary>
    public interface RocketCommand
    {
        /// <summary>
        /// This is the method that will be run when the command is executed
        /// </summary>
        /// <param name="caller">The SteamPlayerID of the caller</param>
        /// <param name="command">The full commandstring</param>
        void Execute(SteamPlayerID caller, string command);

        /// <summary>
        /// The actual commandname that triggers this command
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The helptext of this command
        /// </summary>
        string Help { get; }
    }
}
