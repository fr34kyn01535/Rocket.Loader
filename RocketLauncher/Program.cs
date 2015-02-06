using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Serialization;

namespace RocketLauncher
{
    class Program
    {
        static string InstanceName;
        static string BuildType;
        public static Process RocketProcess;


        static void Main(string[] args)
        {

            string site = Encoding.UTF8.GetString(new Rocket.RocketAPI.RocketWebClient().DownloadData("http://api.rocket.foundation/"));

            XmlSerializer serializer = new XmlSerializer(typeof(List<Build>));

            List<Build> builds = (List<Build>)serializer.Deserialize(new StringReader(site));


           /* List<Build> p = new List<Build>();
            Build example = new Build() {Type = VersionType.Beta,Url ="http://test.de",Version = "1.0.0.0" };
            p.Add(example);

            serializer.Serialize(new StreamWriter("test.xml"),p);
            */



            InstanceName = (args.Length >= 1 && args[0] != null) ? args[0] : "Rocket";
            BuildType = (args.Length >= 2 && args[1] != null) ? (args[1].ToLower() == "release" || args[1].ToLower() == "beta" ? args[1] : "release") : "release";

            Console.WriteLine("Instance name: " + InstanceName);
            Console.WriteLine("Build type: " + BuildType);

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
