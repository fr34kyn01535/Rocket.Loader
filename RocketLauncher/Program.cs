using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Rocket.RocketLauncher
{
    public class RocketLauncher
    {
        static void Main(string[] args)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "Unturned.exe",
                    Arguments = "/nographics -batchmode +secureserver/test",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    WorkingDirectory = Directory.GetCurrentDirectory(),
                }
            };


            process.OutputDataReceived += process_OutputDataReceived;
            process.Start();

            process.BeginOutputReadLine();
            System.Threading.Thread.Sleep(5000);

            process.WaitForExit();
        }

        static void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }
    }
}
