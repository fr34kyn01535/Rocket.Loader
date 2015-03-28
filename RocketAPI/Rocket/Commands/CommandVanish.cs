using Rocket.Logging;
using Rocket.RocketAPI;
using SDG;
using System;

namespace Rocket.Commands
{
    public class CommandVanish : IRocketCommand
    {
        public bool RunFromConsole
        {
            get { return false; }
        }

        public string Name
        {
            get { return "vanish"; }
        }

        public string Help
        {
            get { return "Are we rushing in or are we goin' sneaky beaky like?";}
        }

        public void Execute(Steamworks.CSteamID caller, string command)
        {
            Player p = PlayerTool.getPlayer(caller);
            RocketPlayerFeatures pf = p.gameObject.transform.GetComponent<RocketPlayerFeatures>();
            if (pf.VanishMode)
            {
                Logger.Log(RocketTranslation.Translate("command_vanish_disable_console", p.SteamChannel.SteamPlayer.SteamPlayerID.CharacterName));
                RocketChatManager.Say(caller, RocketTranslation.Translate("command_vanish_disable_private"));
                pf.VanishMode = false;
            }
            else
            {
                Logger.Log(RocketTranslation.Translate("command_vanish_enable_console", p.SteamChannel.SteamPlayer.SteamPlayerID.CharacterName));
                RocketChatManager.Say(caller, RocketTranslation.Translate("command_vanish_enable_private"));
                pf.VanishMode = true;
            }
        }
    }
}