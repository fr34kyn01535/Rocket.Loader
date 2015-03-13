using Rocket.Logging;
using Rocket.RocketAPI;
using SDG;
using System;

namespace Rocket.Commands
{
    public class CommandGod : Command
    {
        public CommandGod()
        {
            base.commandName = "god";
            base.commandHelp = "Cause you ain't givin a shit";
            base.commandInfo = base.commandName + " - " + base.commandHelp;
        }

        protected override void execute(SteamPlayerID caller, string command)
        {
            if (!RocketCommand.IsPlayer(caller)) return;

            Player p = PlayerTool.getPlayer(caller.CSteamID);
            RocketPlayerFeatures pf = p.gameObject.transform.GetComponent<RocketPlayerFeatures>();
            if (pf.GodMode)
            {
                Logger.Log(RocketTranslation.Translate("command_god_disable_console", caller.CharacterName));
                RocketChatManager.Say(caller.CSteamID, RocketTranslation.Translate("command_god_disable_privatechat"));
                pf.GodMode = false;
            }
            else
            {
                Logger.Log(RocketTranslation.Translate("command_god_enable_console", caller.CharacterName));
                RocketChatManager.Say(caller.CSteamID, RocketTranslation.Translate("command_god_enable_privatechat"));
                pf.GodMode = true;
            }
        }
    }
}