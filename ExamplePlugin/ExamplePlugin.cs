using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Steamworks;
using SDG;
using Rocket.RocketAPI;
using System.Reflection;
using Rocket.RocketAPI.Interfaces;
using Rocket.RocketAPI.Managers;

namespace ExamplePlugin
{
    public class ExamplePlugin : RocketPlugin
    {
        private ExampleConfiguration configuration = ConfigurationManager.LoadConfiguration<ExampleConfiguration>();
     
        string RocketPlugin.Author
        {
            get { return "fr34kyn01535"; }
        }
        
        void RocketPlugin.Load()
        {
            Logger.Log("This is the Testmod load()!" + configuration.bla);
            RocketAPI.Commands.RegisterCommand(new CommandTest());

            RocketAPI.Events.PlayerConnected += onPlayerConnected;
            RocketAPI.Events.PlayerDisconnected += onPlayerDisconnected;
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
