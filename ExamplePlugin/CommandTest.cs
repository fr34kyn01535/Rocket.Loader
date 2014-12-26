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

        public CommandTest(String exec, String info, String help)
        {
            base.commandName = exec;
            base.commandInfo = info;
            base.commandHelp = help;
        }


        protected override void execute(SteamPlayerID m, string s)
        {
            JavaScriptSerializer st = new JavaScriptSerializer();
            st.Serialize(typeof(SDG.Steam));
        }
    }
}
