using Rocket.Unturned.Player;
using System.Collections.Generic;
using UnityEngine;

namespace Rocket.Unturned.Commands
{
    public class CommandBroadcast : IRocketCommand
    {
        public bool RunFromConsole
        {
            get { return true; }
        }

        public string Name
        {
            get { return "broadcast"; }
        }

        public string Help
        {
            get { return "Broadcast a message"; }
        }

        public string Syntax
        {
            get { return "<message> [color]"; }
        }

        public List<string> Aliases
        {
            get { return new List<string>(); }
        }

        public void Execute(UnturnedPlayer caller, string[] command)
        {
            string message = command.GetStringParameter(0);
            Color? color = command.GetColorParameter(1);

            if (message == null)
            {
                RocketChat.Say(caller, U.Translate("command_generic_invalid_parameter"));
                return;
            }

            RocketChat.Say(message, (color.HasValue) ? (Color)color : Color.green);
        }
    }
}