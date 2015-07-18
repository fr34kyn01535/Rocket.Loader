using Rocket.Unturned.Player;
using System.Collections.Generic;

namespace Rocket.Unturned.Commands
{
    public class CommandCompass : IRocketCommand
    {
        public bool RunFromConsole
        {
            get { return false; }
        }
        
        public string Name
        {
            get { return "compass"; }
        }

        public string Help
        {
            get { return "Shows the direction you are facing"; }
        }

        public string Syntax
        {
            get { return "[direction]"; }
        }

        public List<string> Aliases
        {
            get { return new List<string>(); }
        }

        public void Execute(UnturnedPlayer caller, string[] command)
        {
            float currentDirection = caller.Rotation;

            if (command.Length == 1)
            {
                switch (command[0])
                {
                    case "north":
                        currentDirection = 0;
                        break;
                    case "east":
                        currentDirection = 90;
                        break;
                    case "south":
                        currentDirection = 180;
                        break;
                    case "west":
                        currentDirection = 270;
                        break;
                    default:
                        RocketChat.Say(caller, U.Translate("command_generic_invalid_parameter"));
                        return;
                }
                caller.Teleport(caller.Position, currentDirection);
            }
            
            string directionName = "Unknown";

            if (currentDirection > 30 && currentDirection < 60)
            {
                directionName = U.Translate("command_compass_northeast");
            }
            else if (currentDirection > 60 && currentDirection < 120)
            {
                directionName = U.Translate("command_compass_east");
            }
            else if (currentDirection > 120 && currentDirection < 150)
            {
                directionName = U.Translate("command_compass_southeast");
            }
            else if (currentDirection > 150 && currentDirection < 210)
            {
                directionName = U.Translate("command_compass_south");
            }
            else if (currentDirection > 210 && currentDirection < 240)
            {
                directionName = U.Translate("command_compass_southwest");
            }
            else if (currentDirection > 240 && currentDirection < 300)
            {
                directionName = U.Translate("command_compass_west");
            }
            else if (currentDirection > 300 && currentDirection < 330)
            {
                directionName = U.Translate("command_compass_northwest");
            }
            else if (currentDirection > 330 || currentDirection < 30)
            {
                directionName = U.Translate("command_compass_north");
            }

            RocketChat.Say(caller, U.Translate("command_compass_facing_private", directionName));
        }
    }
}