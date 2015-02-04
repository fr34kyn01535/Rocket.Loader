using Rocket.Logging;
using Rocket.RocketAPI;
using SDG;

namespace Rocket.Commands
{
    public class CommandI : Command
    {
        public CommandI()
        {
            base.commandName = "i";
            base.commandHelp = "Gives yourself an item";
            base.commandInfo = base.commandName + " - " + base.commandHelp;
        }

        protected override void execute(SteamPlayerID caller, string command)
        {
            string[] componentsFromSerial = Parser.getComponentsFromSerial(command, '/');

            if (componentsFromSerial.Length == 0 || componentsFromSerial.Length > 2)
            {
                RocketChatManager.Say(caller.CSteamID, "Invalid Parameter");
                return;
            }

            ushort id = 0;
            byte amount = 1;

            if (!ushort.TryParse(componentsFromSerial[0].ToString(), out id))
            {
                RocketChatManager.Say(caller.CSteamID, "Invalid Parameter");
                return;
            }

            if (componentsFromSerial.Length == 2 && !byte.TryParse(componentsFromSerial[1].ToString(), out amount))
            {
                RocketChatManager.Say(caller.CSteamID, "Invalid Parameter");
                return;
            }

            SDG.Player player = PlayerTool.getPlayer(caller.CSteamID);
            if (ItemTool.tryForceGiveItem(player, id, amount))
            {
                Logger.Log("Giving " + caller.CharacterName +" item " + id + ":" + amount);
                RocketChatManager.Say(caller.CSteamID, "Giving you item " + id + ":" + amount);
            }
            else
            {
                RocketChatManager.Say(caller.CSteamID, "Failed giving item " + id + ":" + amount);
            }
        }
    }
}