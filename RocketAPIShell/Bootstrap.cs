using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.IO;
namespace Rocket.RocketAPI
{
    public class Bootstrap : MonoBehaviour
    {
        private static Bootstrap instance = null;

        public static void LaunchRocket()
        {
            instance = new Bootstrap();
            InitializeBootstrap();
        }

        public static void InitializeBootstrap() {
           
        }
    }
}
