using Rocket.Logging;
using Rocket.RocketAPI;
using SDG;
using System;

namespace Rocket.Commands
{
    public class CommandVanish : Command
    {
        public CommandVanish()
        {
            base.commandName = "vanish";
            base.commandHelp = "Are we rushing in or are we goin' sneaky beaky like?";
            base.commandInfo = base.commandName + " - " + base.commandHelp;
        }

        protected override void execute(SteamPlayerID caller, string command)
        {
            if (!RocketCommand.IsPlayer(caller)) return;

            Player p = PlayerTool.getPlayer(caller.CSteamID);
            RocketPlayerFeatures pf = p.gameObject.transform.GetComponent<RocketPlayerFeatures>();
            if (pf.VanishMode)
            {
                Logger.Log(RocketTranslation.Translate("command_vanish_disable_console", caller.CharacterName));
                RocketChatManager.Say(caller.CSteamID, RocketTranslation.Translate("command_vanish_disable_private"));
                pf.VanishMode = false;
            }
            else
            {
                Logger.Log(RocketTranslation.Translate("command_vanish_enable_console", caller.CharacterName));
                RocketChatManager.Say(caller.CSteamID, RocketTranslation.Translate("command_vanish_enable_private"));
                pf.VanishMode = true;
            }
        }
    }
}