using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rocket.Unturned.Commands
{
    public interface IRocketCommand
    {
        bool RunFromConsole { get; }
        string Name { get; }
        string Help { get; }
        string Syntax { get; }
        List<string> Aliases { get; }

        void Execute(RocketPlayer caller, string[] command); 
    }
}
