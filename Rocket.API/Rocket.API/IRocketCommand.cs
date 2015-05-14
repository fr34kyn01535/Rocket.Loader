using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rocket.API
{
    public interface IRocketCommand
    {
        bool RunFromConsole { get; }
        string Name { get; }
        string Help { get; }
        void Execute(IRocketPlayer caller, string[] command); //TODO: Make caller nullable
    }
}
