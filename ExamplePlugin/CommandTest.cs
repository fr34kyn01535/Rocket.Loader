using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG;
using UnityEngine;
using Rocket.RocketAPI;
using System.Web.Script.Serialization;

namespace ExamplePlugin
{
    class CommandTest : Command
    {

        public CommandTest()
        {
            base.commandName = "test";
            base.commandInfo = "testcmd - testinfo";
            base.commandHelp = "testcmd - testhelp";
        }


        protected override void execute(SteamPlayerID m, string s)
        {
            ItemManager i = new ItemManager();
            if (s.Contains("1"))
            {
                i.W();
            }
            if (s.Contains("2"))
            {
                i.g();
            }
            if (s.Contains("3"))
            {
                i.P();
            }
        }
    }
}
