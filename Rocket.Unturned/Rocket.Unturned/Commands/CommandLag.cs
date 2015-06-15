using Rocket.API;
using Rocket.Core;
using Rocket.Core.Translations;
using Rocket.Unturned.Player;
using SDG;
using System.Collections.Generic;

namespace Rocket.Unturned.Commands
{
    public class CommandTps : IRocketCommand
    {
        public bool RunFromConsole
        {
            get { return true; }
        }

        public string Name
        {
            get { return "tps"; }
        }

        public string Help
        {
            get { return "Shows the servers TPS";}
        }

        public string Syntax
        {
            get { return ""; }
        }

        public List<string> Aliases
        {
            get { return new List<string>(); }
        }

        public void Execute(RocketPlayer caller, string[] command)
        {
            RocketChat.Say(caller, RocketTranslationManager.Translate("command_tps_tps", RocketBootstrap.TPS));
            RocketChat.Say(caller, RocketTranslationManager.Translate("command_tps_running_since", RocketBootstrap.Started.ToString()));
        }
    }
}