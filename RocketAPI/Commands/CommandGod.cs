using Rocket.RocketAPI;
using SDG;

namespace Rocket
{
    public class CommandGod : Command
    {
        public CommandGod()
        {
            base.commandName = "god";
            base.commandHelp = "Cause you ain't givin a shit";
            base.commandInfo = base.commandName + " - " + base.commandHelp;
        }

        protected override void execute(SteamPlayerID caller, string command)
        {
            Player p = PlayerTool.getPlayer(caller.CSteamID);
            RocketPlayerFeatures pf = p.gameObject.transform.GetComponent<RocketPlayerFeatures>();
            if (pf.GodMode)
            {
                RocketChatManager.Say(caller.CSteamID, "The godly powers left you...");
                pf.GodMode = false;
            }
            else
            {
                RocketChatManager.Say(caller.CSteamID, "You can feel the strength now...");
                pf.GodMode = true;
            }
        }
    }
}