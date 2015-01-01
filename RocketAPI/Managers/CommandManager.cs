using Rocket.RocketAPI.Commands;
using Rocket.RocketAPI.Interfaces;
using SDG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Rocket.RocketAPI.Managers
{
    public class CommandManager
    {
        internal List<RocketCommand> commands = new List<RocketCommand>();
        internal CommandManager()
        {
            loadCommands();
        }

        /// <summary>
        /// This methods allows you to register a new RocketCommand
        /// </summary>
        /// <param name="command">The RocketCommand to register</param>
        public void RegisterCommand(RocketCommand command)
        {
            if (commands.Select(c => c.Name.ToLower()).ToList().Contains(command.Name.ToLower())){
                Logger.Log("Command already registered: " + command.GetType().FullName);
                return;
            }
            commands.Add(command);
        }

        internal void Reload() {
            commands.Clear();
            loadCommands();
        }

        internal void loadCommands()
        {
            IEnumerable<RocketCommand> commandTypes = from t in Assembly.GetExecutingAssembly().GetTypes()
                          where t.GetInterfaces().Contains(typeof(RocketCommand)) && t.GetConstructor(Type.EmptyTypes) != null
                          select Activator.CreateInstance(t) as RocketCommand;

            foreach (RocketCommand command in commandTypes)
            {
                RegisterCommand(command);
            }
        }
    }
}
