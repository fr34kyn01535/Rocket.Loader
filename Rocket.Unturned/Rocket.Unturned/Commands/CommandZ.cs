using Rocket.API;
using Rocket.Core;
using Rocket.Core.Translations;
using Rocket.Unturned.Logging;
using Rocket.Unturned.Player;
using SDG;
using System;
using System.Collections.Generic;

namespace Rocket.Unturned.Commands
{
    public class CommandZ : IRocketCommand
    {
        public bool RunFromConsole
        {
            get { return false; }
        }

        public string Name
        {
            get { return "z"; }
        }

        public string Help
        {
            get { return "Gives yourself a zombie";}
        }

        public string Syntax
        {
            get { return "<amount>"; }
        }

        public List<string> Aliases
        {
            get { return new List<string>(); }
        }

        public void Execute(RocketPlayer caller, string[] command)
        {
            byte amount = 0;
            if(command.Length == 0 || !byte.TryParse(command[0],out amount) || amount > 32){
                RocketChat.Say(caller, RocketTranslationManager.Translate("command_generic_invalid_parameter"));
                return;
            }

            caller.GiveZombie(amount);
            Logger.Log(RocketTranslationManager.Translate("command_z_giving_console",caller.CharacterName,amount));
            RocketChat.Say(caller, RocketTranslationManager.Translate("command_z_giving_private", amount));
        }
    }
}