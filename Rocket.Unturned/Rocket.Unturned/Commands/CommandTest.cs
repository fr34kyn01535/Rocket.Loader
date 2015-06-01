using SDG;
using System;
using UnityEngine;
using System.Linq;
using Rocket.Unturned.Logging;
using Rocket.Core;
using Rocket.API;
using Rocket.Core.Translations;
using Rocket.Unturned.Player;

namespace Rocket.Unturned.Commands
{
#if DEBUG
    public class CommandTest : IRocketCommand
    {
        public bool RunFromConsole
        {
            get { return false; }
        }

        public string Name
        {
            get { return "test"; }
        }

        public string Help
        {
            get { return "";}
        }

        public string Syntax
        {
            get { return ""; }
        }

        public void Execute(RocketPlayer caller, string[] command)
        {
            //Trigger effect
            //Logger.Log(EffectManager.c + " - ");
            //if(command.Length == 1)
            //    EffectManager.sendEffect((ushort)command.GetInt32Parameter(0), EffectManager.c, caller.Player.transform.position);


        }
    }
#endif
}