using Rocket.RocketAPI.Commands;
using SDG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rocket.RocketAPI
{
    public class CommandManager
    {
        internal List<RocketCommand> commands = new List<RocketCommand>();
        public CommandManager()
        {
            RegisterCommand(new CommandReload());
            RegisterCommand(new CommandPlugins());
            RegisterCommand(new CommandCheck());
        }

        public void RegisterCommand(RocketCommand command)
        {
            if (commands.Select(c => c.Name.ToLower()).ToList().Contains(command.Name.ToLower())){
                Logger.Log("Command already registered: " + command.GetType().FullName);
                return;
            }
            commands.Add(command);
        }

        internal void Reload()
        {
            commands.Clear();
        }
    }
}
