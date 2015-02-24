using Rocket.RocketAPI;
using SDG;

namespace Rocket.Commands
{
    public class CommandLag : Command
    {
        public CommandLag()
        {
            base.commandName = "lag";
            base.commandHelp = "Shows the servers TPS";
            base.commandInfo = base.commandName + " - " + base.commandHelp;
        }

        protected override void execute(SteamPlayerID caller, string command)
        {
            RocketChatManager.Say(caller.CSteamID, "TPS: " + Bootstrap.TPS);
            RocketChatManager.Say(caller.CSteamID, "Running since: " + Bootstrap.Started.ToString() + " UTC");
        }
    }
}