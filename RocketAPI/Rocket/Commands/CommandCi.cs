//using Rocket.Logging;
//using Rocket.RocketAPI;
//using SDG;
//using System;

//namespace Rocket.Commands
//{
//    public class CommandCi : IRocketCommand
//    {
//        public bool RunFromConsole
//        {
//            get { return false; }
//        }

//        public string Name
//        {
//            get { return "ci"; }
//        }

//        public string Help
//        {
//            get { return "Clears your inventory"; }
//        }

//        public void Execute(RocketPlayer caller, string command)
//        {
//            caller.Inventory.updateItems(
//            RocketChatManager.Say(caller, RocketTranslation.Translate("command_clear_private"));
//        }
//    }
//}