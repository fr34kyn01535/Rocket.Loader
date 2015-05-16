using Rocket.API;
using Rocket.Core;
using Rocket.Core.Translations;
using Rocket.Unturned.Player;
using SDG;

namespace Rocket.Unturned.Commands
{
    public class CommandLag : IRocketCommand
    {
        public bool RunFromConsole
        {
            get { return true; }
        }

        public string Name
        {
            get { return "lag"; }
        }

        public string Help
        {
            get { return "Shows the servers TPS";}
        }

        public void Execute(RocketPlayer caller, string[] command)
        {
            RocketChat.Say(caller, RocketTranslationManager.Translate("command_tps_tps", RocketBootstrap.TPS));
            RocketChat.Say(caller, RocketTranslationManager.Translate("command_tps_running_since", RocketBootstrap.Started.ToString()));
        }
    }
}