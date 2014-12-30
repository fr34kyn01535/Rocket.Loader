﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.IO;
using SDG;
using Steamworks;
using System.Collections;
using System.Timers;
namespace Rocket.RocketAPI
{
    public static class Bootstrap
    {
        public static GameObject RocketAPIObject;
        public static RocketAPI RocketAPI;

        public static string InstanceName;
        public static string HomeFolder;

        public static void LaunchRocket()
        {
            try
            {
                ESteamSecurity security;
                CommandLine.tryGetServer(out security, out InstanceName);
                HomeFolder = "Servers/" + Bootstrap.InstanceName + "/Rocket/";

                if (!Directory.Exists(HomeFolder)) Directory.CreateDirectory(HomeFolder);

                RocketAPI = new RocketAPI();
            }
            catch (Exception e)
            {
                Logger.LogError("Error while loading Bootstrap: " + e.ToString());
            }
        }

        /*This method will be injected into Unturned, do not touch!*/
        public static System.Byte[] getAssemblyHash()
        {
            byte[] b = new System.Byte[20];
            return b;
        }
    }
}
