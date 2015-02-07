using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace RocketLauncher
{
    class Program
    {
        private static string InstanceName;
        private static Process RocketProcess;

        private static string betaUrl = "https://ci.rocket.foundation/job/Rocket%20Beta/rssAll";
        private static string releaseUrl = "https://ci.rocket.foundation/job/Rocket%20Release/rssAll";
        private static string xmlUrl;

        private static XNamespace atomNamespace = "http://www.w3.org/2005/Atom";

        private static string zipFile = "artifact/RocketLoader/bin/Release/Rocket.zip";

        static void Main(string[] args)
        {
            InstanceName = (args.Length >= 1 && args[0] != null) ? args[0] : "Rocket";

            xmlUrl = (args.Length >= 2 && args[1] != null) ? (args[1].ToLower() == "beta" ? betaUrl : releaseUrl) : releaseUrl;

            XDocument xmlDoc = XDocument.Load(xmlUrl);
            XElement first = xmlDoc.Element(atomNamespace + "feed").Elements(atomNamespace + "entry").First();

            string latestTitle = first.Element(atomNamespace + "title").Value.Split('(')[0].Trim();
            string latestUrl = first.Element(atomNamespace + "link").Attribute("href").Value;


            Console.ReadLine();

            return;


            Console.WriteLine("Instance name: " + InstanceName);

            RocketProcess = new Process();
            RocketProcess.StartInfo.FileName = "Unturned.exe";
            RocketProcess.StartInfo.Arguments = "/nographics -batchmode +secureserver/" + InstanceName;

            RocketProcess.StartInfo.UseShellExecute = true;
            RocketProcess.StartInfo.ErrorDialog = false;

            RocketProcess.StartInfo.RedirectStandardError = false;
            RocketProcess.StartInfo.RedirectStandardInput = false;
            RocketProcess.StartInfo.RedirectStandardOutput = false;
            RocketProcess.StartInfo.CreateNoWindow = true;
            //RocketProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            if (!RocketProcess.Start())
            {
                Console.WriteLine("Error - Press any key to close");
                Console.ReadKey();
            }

            /*
            string cmd;
            bool exit = false;
            while (!exit)
            {
                cmd = Console.ReadLine();
                Console.WriteLine(">>" + cmd);
            }*/

        }
    }
}
