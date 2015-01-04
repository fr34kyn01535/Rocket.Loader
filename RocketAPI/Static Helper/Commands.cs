using SDG;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rocket
{
    public class Commands : MonoBehaviour
    {
        internal static void RegisterCommand(Command command)
        {
            List<Command> commandList = new List<Command>();
            bool msg = false;
            foreach (Command ccommand in Commander.commandList)
            {
                if (ccommand.commandName.ToLower().Equals(command.commandName.ToLower()))
                {
                    if (ccommand.GetType().Assembly.GetName().Name == "Assembly-CSharp")
                    {
                        Logger.LogWarning(command.GetType().Assembly.GetName().Name + "." + command.commandName+" overwrites built in command " + ccommand.commandName);
                        msg = true;
                    }
                    else
                    {
                        Logger.LogError("Can not register command " + command.GetType().Assembly.GetName().Name + "." + command.commandName + " because its already registered by " + ccommand.GetType().Assembly.GetName().Name + "." + ccommand.commandName);
                        return;
                    }
                }
                else {
                    commandList.Add(ccommand);
                }
            }

            if(!msg) Logger.Log(command.GetType().Assembly.GetName().Name + "." + command.commandName);
            commandList.Add(command);
            Commander.commandList = commandList.ToArray();
        }
    }

}
