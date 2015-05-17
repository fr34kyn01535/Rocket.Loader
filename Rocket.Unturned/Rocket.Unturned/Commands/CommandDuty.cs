using SDG;
using System;
using Rocket.API;
using Rocket.Unturned.Logging;
using Rocket.Core;
using Rocket.Unturned.Events;
using Rocket.Core.Translations;
using Rocket.Unturned.Player;

namespace Rocket.Unturned.Commands
{
    public class CommandDuty : IRocketCommand
    {
        public bool RunFromConsole
        {
            get { return false; }
        }

        public string Name
        {
            get { return "duty"; }
        }

        public string Help
        {
            get { return "Admin yourself, promise you will not abuse it ;)";}
        }

        public void Execute(RocketPlayer caller, string[] command)
        {
            if (caller.IsAdmin)
            {
                Logger.Log(RocketTranslationManager.Translate("command_duty_disable_console", caller.CharacterName));
                RocketChat.Say(caller, RocketTranslationManager.Translate("command_duty_disable_private"));
                caller.Admin(false);
                caller.Features.GodMode = false;
                caller.Features.VanishMode = false;
            }
            else
            {
                Logger.Log(RocketTranslationManager.Translate("command_duty_enable_console", caller.CharacterName));
                RocketChat.Say(caller, RocketTranslationManager.Translate("command_duty_enable_private"));
                caller.Admin(true, caller);
            }

            RocketServerEvents.OnPlayerDisconnected += (RocketPlayer player) =>
            {
                if (caller == player)
                {
                    Logger.Log(RocketTranslationManager.Translate("command_duty_disable_console", player.CharacterName));
                    RocketChat.Say(caller, RocketTranslationManager.Translate("command_duty_disable_private"));
                    caller.Admin(false);
                }
            };            
        }
    }
}