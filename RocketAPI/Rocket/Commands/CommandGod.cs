using Rocket.Logging;
using Rocket.RocketAPI;
using SDG;
using System;

namespace Rocket.Commands
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

        public void Execute(Steamworks.CSteamID caller, string command)
        {
            Player p = PlayerTool.getPlayer(caller);
            RocketPlayerFeatures pf = p.gameObject.transform.GetComponent<RocketPlayerFeatures>();
            if (pf.GodMode)
            {
                Logger.Log(RocketTranslation.Translate("command_god_disable_console", p.SteamChannel.SteamPlayer.SteamPlayerID.CharacterName));
                RocketChatManager.Say(caller, RocketTranslation.Translate("command_god_disable_private"));
                pf.GodMode = false;
            }
            else
            {
                Logger.Log(RocketTranslation.Translate("command_god_enable_console", p.SteamChannel.SteamPlayer.SteamPlayerID.CharacterName));
                RocketChatManager.Say(caller, RocketTranslation.Translate("command_god_enable_private"));
                pf.GodMode = true;
            }
        }
    }
}