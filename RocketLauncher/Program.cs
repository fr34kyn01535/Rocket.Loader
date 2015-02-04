using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;

namespace RocketLauncher
{
    class Program
    {
        static string InstanceName;
        public static Process RocketProcess;


        static void Main(string[] args)
        {
            InstanceName = (args.Length >= 1) ? args[0] : "Rocket";
            /*
            RocketProcess = new Process();
            RocketProcess.StartInfo.FileName = "Unturned.exe";
            RocketProcess.StartInfo.Arguments = "/nographics -batchmode +secureserver/" + InstanceName;

            RocketProcess.StartInfo.UseShellExecute = true;
            RocketProcess.StartInfo.ErrorDialog = false;

            RocketProcess.StartInfo.RedirectStandardError = false;
            RocketProcess.StartInfo.RedirectStandardInput = false;
            RocketProcess.StartInfo.RedirectStandardOutput = false;
            RocketProcess.StartInfo.CreateNoWindow = true;
            RocketProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            if (!RocketProcess.Start())
            {
                Console.WriteLine("Error");
            }*/

            string cmd;
            bool exit = false;
            while (!exit)
            {
                cmd = Console.ReadLine();
                Console.WriteLine(">>" + cmd);
            }
        }
    }
}
