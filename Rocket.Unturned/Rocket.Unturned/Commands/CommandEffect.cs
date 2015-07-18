using Rocket.Unturned.Player;
using System.Collections.Generic;

namespace Rocket.Unturned.Commands
{
    public class CommandEffect : IRocketCommand
    {
        public bool RunFromConsole
        {
            get { return false; }
        }

        public string Name
        {
            get { return "effect"; }
        }

        public string Help
        {
            get { return "Triggers an effect at your position";}
        }

        public string Syntax
        {
            get { return "<id>"; }
        }

        public List<string> Aliases
        {
            get { return new List<string>(); }
        }

        public void Execute(UnturnedPlayer caller, string[] command)
        {
            ushort? id = command.GetUInt16Parameter(0);
            if (id == null)
            {
                RocketChat.Say(caller, U.Translate("command_generic_invalid_parameter"));
                return;
            }
            caller.TriggerEffect(id.Value);
        }
    }
}