using Rocket.RocketAPI;
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
            RocketChatManager.Say(caller, RocketTranslation.Translate("command_tps_tps", Rocket.TPS));
            RocketChatManager.Say(caller, RocketTranslation.Translate("command_tps_running_since", Rocket.Started.ToString()));
        }
    }
}