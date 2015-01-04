using SDG;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rocket
{
    public class Commands : MonoBehaviour
    {
        public static void RegisterCommand(Command command)
        {
            foreach (Command ccommand in Commander.commandList)
            if (ccommand.commandName.ToLower().Equals(command.commandName.ToLower()))
            {
                Logger.Log("Command already registered: " + command.GetType().FullName);
                return;
            }

            List<Command> commandList = Commander.commandList.ToList();
            commandList.Add(command);
            Commander.commandList = commandList.ToArray();
        }
    }

}
