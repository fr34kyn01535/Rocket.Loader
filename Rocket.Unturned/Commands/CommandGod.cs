using Rocket.Logging;
using Rocket.RocketAPI;
using SDG;
using System;

namespace Rocket.Unturned.Commands
{
    public class CommandGod : IRocketCommand
    {
        public bool RunFromConsole
        {
            get { return false; }
        }

        public string Name
        {
            get { return "god"; }
        }

        public string Help
        {
            get { return "Cause you ain't givin a shit";}
        }

        public void Execute(RocketPlayer caller, string[] command)
        {
            if (caller.Features.GodMode)
            {
                Logger.Log(RocketTranslation.Translate("command_god_disable_console", caller.CharacterName));
                RocketChatManager.Say(caller, RocketTranslation.Translate("command_god_disable_private"));
                caller.Features.GodMode = false;
            }
            else
            {
                Logger.Log(RocketTranslation.Translate("command_god_enable_console", caller.CharacterName));
                RocketChatManager.Say(caller, RocketTranslation.Translate("command_god_enable_private"));
                caller.Features.GodMode = true;
            }
        }
    }
}