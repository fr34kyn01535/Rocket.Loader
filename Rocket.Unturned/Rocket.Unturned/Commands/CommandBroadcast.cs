using Rocket.API;
using Rocket.Core;
using Rocket.Core.Translations;
using Rocket.Unturned.Logging;
using Rocket.Unturned.Player;
using SDG;
using System;
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

        public void Execute(RocketPlayer caller, string[] command)
        {
            string message = command.GetStringParameter(0);
            Color? color = command.GetColorParameter(1);

            if (message == null)
            {
                RocketChat.Say(caller, RocketTranslationManager.Translate("command_generic_invalid_parameter"));
                return;
            }

            RocketChat.Say(message, (color.HasValue) ? (Color)color : Color.green);
        }
    }
}