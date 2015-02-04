using Rocket.RocketAPI;
using SDG;

namespace Rocket.Commands
{
    public class CommandRocket : Command
    {
        public CommandRocket()
        {
            base.commandName = "rocket";
            base.commandHelp = "About us :)";
            base.commandInfo = base.commandName + " - " + base.commandHelp;
        }

        protected override void execute(SteamPlayerID caller, string command)
        {
            RocketChatManager.Say(caller.CSteamID, "Rocket was brought to you by Kunii & fr34kyn01535");
            RocketChatManager.Say(caller.CSteamID, "https://rocket.foundation © 2015");
            RocketChatManager.Say(caller.CSteamID, "Visit our servers at https://unturned.rocks");
        }
    }
}