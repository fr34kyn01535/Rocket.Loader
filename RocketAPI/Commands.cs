using SDG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rocket.RocketAPI
{
    public static class Commands
    {
       public static void RegisterCommand(Command command, bool checkForDupe = false)
        {
            foreach (Command ccommand in Commander.commandList)
                if (ccommand.commandName.ToLower().Equals(command.commandName.ToLower()))
                {
                    //Logger.Log("Command already registered: " + command.GetType().FullName);
                    return;
                }

            List<Command> commandList = Commander.commandList.ToList();
            commandList.Add(command);
            Commander.commandList = commandList.ToArray();
        }
    }
}
