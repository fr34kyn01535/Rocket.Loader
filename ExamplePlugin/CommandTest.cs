using Rocket;
using SDG;

namespace ExamplePlugin
{
    class CommandTest : Command
    {
        public CommandTest()
        {
            base.commandName = "test";
            base.commandInfo = base.commandHelp = "This is a testcommand";
        }

        protected override void execute(SteamPlayerID caller, string command)
        {

        }
    }
}
