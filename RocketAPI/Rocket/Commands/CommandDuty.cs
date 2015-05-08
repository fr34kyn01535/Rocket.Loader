using Rocket.Logging;
using Rocket.RocketAPI;
using Rocket.RocketAPI.Events;
using SDG;
using System;

namespace Rocket.Commands
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
                Logger.Log(RocketTranslation.Translate("command_duty_disable_console", caller.CharacterName));
                RocketChatManager.Say(caller, RocketTranslation.Translate("command_duty_disable_private"));
                caller.Admin(false);
                caller.Features.GodMode = false;
                caller.Features.VanishMode = false;
            }
            else
            {
                Logger.Log(RocketTranslation.Translate("command_duty_enable_console", caller.CharacterName));
                RocketChatManager.Say(caller, RocketTranslation.Translate("command_duty_enable_private"));
                caller.Admin(true,caller);
            }

            RocketServerEvents.OnPlayerDisconnected += (RocketPlayer player) =>
            {
                if (player == caller)
                {
                    Logger.Log(RocketTranslation.Translate("command_duty_disable_console", player.CharacterName));
                    RocketChatManager.Say(caller, RocketTranslation.Translate("command_duty_disable_private"));
                    caller.Admin(false);
                }
            };            
        }
    }
}