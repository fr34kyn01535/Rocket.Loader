using Rocket.Logging;
using Rocket.RocketAPI;
using SDG;
using System;

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

        public void Execute(RocketPlayer caller, string[] command)
        {
            byte amount = 0;
            if(command.Length == 0 || !byte.TryParse(command[0],out amount) || amount > 32){
                RocketChatManager.Say(caller, RocketTranslation.Translate("command_generic_invalid_parameter"));
                return;
            }

            caller.GiveZombie(amount);
            Logger.Log(RocketTranslation.Translate("command_z_giving_console",caller.CharacterName,amount));
            RocketChatManager.Say(caller, RocketTranslation.Translate("command_z_giving_private", amount));
        }
    }
}