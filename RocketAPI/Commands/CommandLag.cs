using Rocket.RocketAPI;
using SDG;

namespace Rocket
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
            RocketChatManager.Say(caller.CSteamID, "TPS: " + RocketLauncher.TPS+" FTPS: "+RocketLauncher.FTPS);
            RocketChatManager.Say(caller.CSteamID, "Running since: " + RocketLauncher.Started.ToString() + " UTC");
        }
    }
}