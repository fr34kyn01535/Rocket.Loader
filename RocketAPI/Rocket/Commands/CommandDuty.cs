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

        public void Execute(Steamworks.CSteamID caller, string command)
        {
            Player p = PlayerTool.getPlayer(caller);
            RocketPlayerFeatures pf = p.transform.GetComponent<RocketPlayerFeatures>();

            if (p.SteamChannel.SteamPlayer.IsAdmin)
            {
                Logger.Log(RocketTranslation.Translate("command_duty_disable_console", p.SteamChannel.SteamPlayer.SteamPlayerID.CharacterName));
                RocketChatManager.Say(caller, RocketTranslation.Translate("command_duty_disable_private"));
                SteamAdminlist.unadmin(caller);
                pf.GodMode = false;
                pf.VanishMode = false;
            }
            else
            {
                Logger.Log(RocketTranslation.Translate("command_duty_enable_console", p.SteamChannel.SteamPlayer.SteamPlayerID.CharacterName));
                RocketChatManager.Say(caller, RocketTranslation.Translate("command_duty_enable_private"));
                SteamAdminlist.admin(caller, caller);
            }

            RocketServerEvents.OnPlayerDisconnected += (Player player) =>
            {
                if (player.SteamChannel.SteamPlayer.SteamPlayerID.CSteamID == caller)
                {
                    Logger.Log(RocketTranslation.Translate("command_duty_disable_console", p.SteamChannel.SteamPlayer.SteamPlayerID.CharacterName));
                    RocketChatManager.Say(caller, RocketTranslation.Translate("command_duty_disable_private"));
                    SteamAdminlist.unadmin(caller);
                }
            };            
        }
    }
}