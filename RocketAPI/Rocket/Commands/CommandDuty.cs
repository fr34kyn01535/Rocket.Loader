using Rocket.Logging;
using Rocket.RocketAPI;
using Rocket.RocketAPI.Events;
using SDG;
using System;

namespace Rocket.Commands
{
    public class CommandDuty : Command
    {
        public CommandDuty()
        {
            base.commandName = "duty";
            base.commandHelp = "Admin yourself, promise you will not abuse it ;)";
            base.commandInfo = base.commandName + " - " + base.commandHelp;
        }

        protected override void execute(SteamPlayerID caller, string command)
        {
            if (!RocketCommand.IsPlayer(caller)) return;

            Player p = PlayerTool.getPlayer(caller.CSteamID);

            if (p.SteamChannel.SteamPlayer.IsAdmin)
            {
                Logger.Log(RocketTranslation.Translate("command_duty_disable_console", caller.CharacterName));
                RocketChatManager.Say(caller.CSteamID, RocketTranslation.Translate("command_duty_disable_private"));
                SteamAdminlist.unadmin(caller);
            }
            else
            {
                Logger.Log(RocketTranslation.Translate("command_duty_enable_console", caller.CharacterName));
                RocketChatManager.Say(caller.CSteamID, RocketTranslation.Translate("command_duty_enable_private"));
                SteamAdminlist.admin(caller, caller);
            }

            RocketServerEvents.OnPlayerDisconnected += (Player player) =>
            {
                if (player.SteamChannel.SteamPlayer.SteamPlayerID.CSteamID == caller.CSteamID)
                {
                    Logger.Log(RocketTranslation.Translate("command_duty_disable_console", caller.CharacterName));
                    RocketChatManager.Say(caller.CSteamID, RocketTranslation.Translate("command_duty_disable_private"));
                    SteamAdminlist.unadmin(caller);
                }
            };            
        }
    }
}