using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG;
using UnityEngine;
using Rocket.RocketAPI;
using System.Web.Script.Serialization;
using Rocket.RocketAPI.Interfaces;

namespace ExamplePlugin
{
    class CommandTest : RocketCommand
    {
        public void Execute(SteamPlayerID caller, string command)
        {
            ChatManager.say("Test");
        }

        public string Name
        {
            get { return "Test"; }
        }

        public string Help
        {
            get { return "This is a helpmessage"; }
        }
    }
}
