﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Steamworks;
using SDG;
using Rocket.RocketAPI;
using System.Reflection;

namespace ExamplePlugin
{
    public class ExamplePlugin : RocketPlugin
    {

        private ExampleConfiguration configuration = RocketConfiguration.LoadConfiguration<ExampleConfiguration>();
     
        string RocketPlugin.Author
        {
            get { return "fr34kyn01535"; }
        }
        
        void RocketPlugin.Load()
        {
            Logger.Log("This is the Testmod load()!" + configuration.bla);
            Core.Commands.Add(new CommandTest());

            SDG.Steam.serverConnected += onPlayerConnected;
            SDG.Steam.serverDisconnected += onPlayerDisconnected;

        }

        static void onPlayerConnected(CSteamID id)
        {
            Logger.Log("onPlayerConnected");
        }

        static void onPlayerDisconnected(CSteamID id)
        {
            Logger.Log("onPlayerDisconnected");
        }

    }
}
