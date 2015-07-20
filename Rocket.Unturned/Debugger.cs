using System;
using System.Diagnostics;
using UnityEngine;

namespace Rocket.Unturned
{
    public class Debugger : MonoBehaviour
    {
        private DateTime lastUpdate = DateTime.Now;
        private byte maxPlayers;

        public void Awake()
        {
            if(Process.GetProcessesByName("devenv").Length == 0)
            {
                U.Initialize();
                Destroy(this);
                return;
            }
            DontDestroyOnLoad(gameObject);
            maxPlayers = SDG.Unturned.Steam.MaxPlayers;
            SDG.Unturned.Steam.MaxPlayers = 0;
            Console.Write("Waiting for debugger...");
        }

        public void FixedUpdate() {
            if((DateTime.Now - lastUpdate).TotalSeconds > 3)
            {
                Console.Write(".");
                lastUpdate = DateTime.Now;
            }

            if(System.Diagnostics.Debugger.IsAttached)
            {
                Console.WriteLine("\nDebugger found, continuing...");
                SDG.Unturned.Steam.MaxPlayers = maxPlayers;
                U.Initialize();
                Destroy(this);
            }
        }
    }
}
