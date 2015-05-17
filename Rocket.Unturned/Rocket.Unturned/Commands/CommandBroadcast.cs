using Rocket.API;
using Rocket.Core;
using Rocket.Core.Translations;
using Rocket.Unturned.Logging;
using Rocket.Unturned.Player;
using SDG;
using System;
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

        public void Execute(RocketPlayer caller, string[] command)
        {
             if (command.Length == 0)
            {
                RocketChat.Say(caller, RocketTranslationManager.Translate("command_generic_invalid_parameter"));
                return;
            }
            Color c = Color.green;
            if(command.Length > 1){
                c = RocketChat.GetColorFromName(command[1],c);
            }
            c = new Color(215, 84, 44,100);
            RocketChat.Say(command[0], c);
        }
    }
}