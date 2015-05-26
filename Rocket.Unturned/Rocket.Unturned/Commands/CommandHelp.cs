using Rocket.API;
using Rocket.Core;
using Rocket.Core.Misc;
using Rocket.Core.Plugins;
using Rocket.Core.Translations;
using Rocket.Unturned.Logging;
using Rocket.Unturned.Player;
using SDG;
using System;
using System.Linq;
using System.Reflection;

namespace Rocket.Unturned.Commands
{
    public class CommandHelp : IRocketCommand
    {
        public bool RunFromConsole
        {
            get { return true; }
        }

        public string Name
        {
            get { return "help"; }
        }

        public string Help
        {
            get { return "Shows you a specific help";}
        }

        public string Syntax
        {
            get { return "[command]"; }
        }

        public void Execute(RocketPlayer caller, string[] command)
        {
            if (command.Length == 0)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("[Vanilla]");
                Console.ForegroundColor = ConsoleColor.White;
                Commander.Commands.Where(c => c.GetType().Assembly == typeof(Commander).Assembly).OrderBy(c => c.commandName).All(c => { Console.WriteLine(c.commandName.ToLower().PadRight(20, ' ') + " " + c.commandInfo.Replace(c.commandName, "").TrimStart().ToLower()); return true; });
                
                Console.WriteLine(); 
                
                Console.ForegroundColor = ConsoleColor.Cyan; 
                Console.WriteLine("[Rocket]");
                Console.ForegroundColor = ConsoleColor.White;
                Commander.Commands.Where(c => c is RocketCommandBase && ((RocketCommandBase)c).Command.GetType().Assembly == Assembly.GetExecutingAssembly()).OrderBy(c => c.commandName).All(c => { Console.WriteLine(c.commandName.ToLower().PadRight(20, ' ') + " " + c.commandInfo.ToLower()); return true; });
                
                Console.WriteLine();
                
                foreach (IRocketPlugin plugin in RocketPluginManager.GetPlugins())
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("[" + plugin.GetType().Assembly.GetName().Name + "]");
                    Console.ForegroundColor = ConsoleColor.White;
                    Commander.Commands.Where(c => c is RocketCommandBase && ((RocketCommandBase)c).Command.GetType().Assembly == plugin.GetType().Assembly).OrderBy(c => c.commandName).All(c => { Console.WriteLine(c.commandName.ToLower().PadRight(20, ' ') + " " + c.commandInfo.ToLower()); return true; });
                    Console.WriteLine();
                }
            }
            else
            {
                Command cmd = Commander.Commands.Where(c => c.commandName.ToLower() == command[0]).FirstOrDefault();
                if (cmd != null)
                {
                    string commandName = cmd.commandName;
                    if (cmd is RocketCommandBase)
                    {
                        commandName = ((RocketCommandBase)cmd).Command.GetType().Assembly.GetName().Name + " / " + cmd.commandName;
                    }
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("[" + commandName + "]");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(cmd.commandName + "\t\t" + cmd.commandInfo);
                    Console.WriteLine(cmd.commandHelp);
                }
            }
        }
    }
}