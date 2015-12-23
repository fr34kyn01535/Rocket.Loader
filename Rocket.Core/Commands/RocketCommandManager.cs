using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using UnityEngine;
using Rocket.API;
using System.Text.RegularExpressions;
using System.Reflection;
using Rocket.Core.Utils;
using Rocket.Core.Logging;

namespace Rocket.Core.Commands
{
    public class RocketCommandManager : MonoBehaviour
    {
        private readonly List<IRocketCommand> commands = new List<IRocketCommand>();
        public ReadOnlyCollection<IRocketCommand> Commands { get; internal set; }

        private void Awake()
        {
            Commands = commands.AsReadOnly();
        }

        private IRocketCommand GetCommand(IRocketCommand command)
        {
           return GetCommand(command.Name);
        }

        private IRocketCommand GetCommand(string command)
        {
            IRocketCommand foundCommand = commands.Where(c => c.Name == command || c.Aliases.Contains(command)).FirstOrDefault();
            return foundCommand;
        }

        public bool Register(IRocketCommand command)
        {
            Logger.Log("Registering " + command.GetType().FullName + " as " + command.Name);
            if (GetCommand(command) != null) return false;
            commands.Add(command);
            return true;
        }

        public void Deregister(IRocketCommand command)
        {
            Logger.Log("Deregister " + command.GetType().FullName + " as "  + command.Name);
            commands.Remove(command);
        }

        public bool Execute(IRocketPlayer player, string command)
        {
            string[] commandParts = Regex.Matches(command, @"[\""](.+?)[\""]|([^ ]+)", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture).Cast<Match>().Select(m => m.Value.Trim('"').Trim()).ToArray();

            if (commandParts.Length != 0)
            {
                name = commandParts[0];
                string[] parameters = commandParts.Skip(1).ToArray();
                if (player == null) player = new ConsolePlayer();
                IRocketCommand rocketCommand = GetCommand(name);
                if (rocketCommand != null)
                {
                    if (!rocketCommand.AllowFromConsole && player is ConsolePlayer)
                    {
                        Logger.Log("This command can't be called from console");
                        return false;
                    }
                    try
                    {
                        rocketCommand.Execute(player, parameters);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError("An error occured while executing " + rocketCommand.Name + " [" + String.Join(", ", parameters) + "]: " + ex.ToString());
                    }
                    return true;
                }
            }

            return false;
        }

        public void RegisterFromAssembly(Assembly assembly)
        {
            List<Type> commands = RocketHelper.GetTypesFromInterface(assembly, "IRocketCommand");
            foreach (Type commandType in commands)
            {
                IRocketCommand command = (IRocketCommand)Activator.CreateInstance(commandType);
                Register(command);
            }
        }

        public void UnregisterFromAssembly(Assembly assembly)
        {
            foreach (IRocketCommand command in R.Commands.Commands.Where(c => c.GetType().Assembly == assembly).ToList())
            {
                Deregister(command);
            }
        }
    }
}
