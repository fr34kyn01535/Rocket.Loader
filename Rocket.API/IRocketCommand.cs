using System.Collections.Generic;

namespace Rocket.API
{
    public interface IRocketCommand
    {
        bool AllowFromConsole { get; }
        string Name { get; }
        string Help { get; }
        string Syntax { get; }
        List<string> Aliases { get; }
        List<string> Permissions { get; }

        void Execute(IRocketPlayer caller, string[] command); 
    }
}
