using System;
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
        string RocketPlugin.Name
        {
            get { return "ExamplePlugin"; }
        }

        string RocketPlugin.Version
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); }
        }

        string RocketPlugin.Author
        {
            get { return "fr34kyn01535"; }
        }

        public ExampleConfiguration config;

        public ExamplePlugin() {
            config = (ExampleConfiguration)(new ExampleConfiguration()).LoadConfiguration(); //Still searching a way to make this easier..
        }


        void RocketPlugin.Load()
        {
            Logger.Log("This is the Testmod load()!" + config.bla);
            Core.RegisterCommand(new CommandTest("testcmd", "testcmd - testinfo", "testcmd - testhelp"));

            /*SDG.Steam.clientConnected += onConnected;
            SDG.Steam.clientDisconnected += onDisconnected;
            SDG.Steam.serverHosted += onHosted;
            SDG.Steam.serverShutdown += onShutdown;
            SDG.Steam.serverConnected += onServerConnected;
            SDG.Steam.serverDisconnected += onServerDisconnected;*/
        }


        public static void onConnected()
        {
            Logger.Log("onConnected!");
        }

        public static void onDisconnected()
        {
            Logger.Log("onDisconnected!");
        }

        public static void onHosted()
        {
            Logger.Log("onHosted!");
        }

        public static void onShutdown()
        {
            Logger.Log("onShutdown!");
        }

        public static void onServerConnected(CSteamID id)
        {
            Logger.Log("onServerConnected!");

            ChatManager.say("WARNING: This is an experimental modded server!");
        }

        public static void onServerDisconnected(CSteamID id)
        {
            Logger.Log("onServerDisconnected!");
        }
    }
}
