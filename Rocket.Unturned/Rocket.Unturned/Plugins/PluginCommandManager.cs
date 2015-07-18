using Rocket.Core.Misc;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Rocket.Unturned.Commands;

namespace Rocket.Unturned.Plugins
{
    public sealed class PluginCommandManager : MonoBehaviour
    {
        private Assembly assembly = Assembly.GetExecutingAssembly();

        private void OnEnable()
        {
            List<Type> commands = RocketHelper.GetTypesFromInterface(assembly, "IRocketCommand");
            foreach (Type command in commands)
            {
                IRocketCommand rocketCommand = (IRocketCommand)Activator.CreateInstance(command);
                Register((Command)(new RocketCommandBase(rocketCommand)));
                foreach (string alias in rocketCommand.Aliases)
                {
                    Register((Command)(new RocketAliasBase(rocketCommand, alias)));
                }
            }
        }

        private void OnDisable()
        {
            foreach(Command c in Commander.Commands.Where(c => (c is RocketCommandBase && ((RocketCommandBase)c).Command.GetType().Assembly == assembly)).ToList()){
                Commander.deregister(c);
            }
        }

        private void Register(Command command)
        {
            string assemblyName = command.GetType().Assembly.GetName().Name;
            List<Command> existingCommand = Commander.Commands.Where(c => (String.Compare(c.commandName, command.commandName, true) == 0)).ToList();
            Commander.register(command);
        }
    }
}