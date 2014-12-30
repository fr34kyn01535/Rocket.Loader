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
    public class Bootstrap : MonoBehaviour
    {
        public static GameObject RocketAPIObject = null;
        public static Core RocketAPI;

        public static string InstanceName = null;

        private static Bootstrap instance = null;

        public static void LaunchRocket()
        {
            if (instance == null)
            {
                instance = new Bootstrap();
                InitializeBootstrap();
            }
        }

        /*This method will be injected into Unturned, do not touch!*/
        public static System.Byte[] getAssemblyHash()
        {
            byte[] b = new System.Byte[20];
            return b;
        }

        public static void InitializeBootstrap()
        {
            try
            {
                ESteamSecurity security;
                CommandLine.tryGetServer(out security, out InstanceName);

                if (!Directory.Exists("Servers/" + Bootstrap.InstanceName + "/Rocket/")) Directory.CreateDirectory("Servers/" + Bootstrap.InstanceName + "/Rocket/");
                if (!Directory.Exists("Servers/" + Bootstrap.InstanceName + "/Rocket/Plugins/")) Directory.CreateDirectory("Servers/" + Bootstrap.InstanceName + "/Rocket/Plugins/");

                UnityEngine.Object.Destroy(RocketAPIObject);
                RocketAPIObject = new GameObject(instance.GetType().FullName);
                RocketAPIObject.AddComponent(instance.GetType());

            }
            catch (Exception e)
            {
                Logger.LogError("Error while loading Bootstrap: " + e.ToString());
            }
        }

        public void Awake()
        {
            UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
            RocketAPI = new Core();
        }
    }
}
