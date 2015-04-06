﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rocket.RocketAPI
{
    public interface IRocketCommand
    {
        bool RunFromConsole { get; }
        string Name { get; }
        string Help { get; }
        void Execute(RocketPlayer caller, string command);
    }
}
