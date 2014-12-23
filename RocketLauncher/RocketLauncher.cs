using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;

namespace Rocket.RocketLauncher
{
    class RocketLauncher
    {

        [DllImport("Kernel32")]
        public static extern bool SetConsoleCtrlHandler(HandlerRoutine Handler, bool Add);

        // A delegate type to be used as the handler routine 
        // for SetConsoleCtrlHandler.
        public delegate bool HandlerRoutine(CtrlTypes CtrlType);

        // An enumerated type for the control messages
        // sent to the handler routine.
        public enum CtrlTypes 
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT,
            CTRL_CLOSE_EVENT, 
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT
        }

        private static bool ConsoleCtrlCheck(CtrlTypes ctrlType)
        {
            try
            {
                Process[] patcherProcess = Process.GetProcessesByName("RocketLoader");
                if (patcherProcess.Length != 0) { Console.WriteLine("RocketLoader already running"); foreach (Process p in patcherProcess) { p.Kill(); } };

                Restore();
            }
            catch (Exception)
            {
            }
            return true;
        }

        
        static string loaderDir = @"Unturned_Data\Rocket";
        static string apiUrl = "https://ci.bam.yt/view/Rocket/job/RocketAPI/lastStableBuild/artifact/RocketAPI/bin/Release/RocketAPI.dll";
        static string loaderUrl = "https://ci.bam.yt/view/Rocket/job/RocketLoader/lastStableBuild/artifact/RocketLoader/bin/Release/RocketLoader.exe";

        static void Restore()
        {
            if (Directory.Exists(@"Unturned_Data\Managed.original"))
            {
                if (Directory.Exists(@"Unturned_Data\Managed"))
                {
                    if (Directory.Exists(loaderDir))
                    {
                        Directory.Delete(@"Unturned_Data\Managed",true);
                        Directory.Move(@"Unturned_Data\Managed.original", @"Unturned_Data\Managed");
                    }
                    else
                    {
                        Directory.Move(@"Unturned_Data\Managed", loaderDir);
                        Directory.Move(@"Unturned_Data\Managed.original", @"Unturned_Data\Managed");
                    }
                }
                else
                {
                    Directory.Move(@"Unturned_Data\Managed.original", @"Unturned_Data\Managed");
                }
            }
        }

        static void Main(string[] args)
        {
            try
            {
                Console.Title = "RocketLauncher";
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(@"                _                                                              ");
                Console.WriteLine(@"               |_)  _   _ |   _ _|_ |   _.     ._   _ |_   _  ._               ");
                Console.WriteLine(@"               | \ (_) (_ |< (/_ |_ |_ (_| |_| | | (_ | | (/_ |                ");
                Console.WriteLine("                                               Version: "+Assembly.GetExecutingAssembly().GetName().Version.ToString());
                Console.ForegroundColor = ConsoleColor.White;
                main(args);
            }

            catch (Exception ex)
            {
                Restore();
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("The Rocket blew up, please fetch a newer version or contact the developers!\n");
                Console.ReadKey();
                Console.WriteLine(ex.ToString());
                Console.ReadKey();
            }
        }

        private static void main(string[] args)
        {
            SetConsoleCtrlHandler(new HandlerRoutine(ConsoleCtrlCheck), true);

            if (!File.Exists("Unturned.exe")) { Console.WriteLine("Unturned.exe not found"); Console.ReadKey(); return; };

            if (Process.GetProcessesByName("Unturned").Length != 0) { Console.WriteLine("Unturned already running"); Console.ReadKey(); return; };
            if (Process.GetProcessesByName("RocketLauncher").Length > 1) { Console.WriteLine("RocketLauncher already running"); Console.ReadKey(); return; };

            Process[] patcherProcess = Process.GetProcessesByName("RocketLoader");
            if (patcherProcess.Length != 0) { Console.WriteLine("RocketLoader already running"); foreach (Process p in patcherProcess) { p.Kill(); } };

            Restore();

            string[] checkFiles = { @"Assembly-CSharp.dll", @"Assembly-CSharp-firstpass.dll", @"Other-Assembly-CSharp.dll", @"Other-Assembly-CSharp-firstpass.dll" };

            foreach (string file in checkFiles)
            {
                if (!File.Exists(Path.Combine(@"Unturned_Data\Managed", file))) { Console.WriteLine(file + " not found"); Console.ReadKey(); return; };
            }

            Console.WriteLine("Checking Rocket folder...");
            if (Directory.Exists(loaderDir))
            {
                Console.Write("exists");
                Console.WriteLine("Checking Rocket version...");
                foreach (string fileName in checkFiles)
                {
                    if (!fileCompare(Path.Combine(loaderDir, fileName + ".original"), Path.Combine(@"Unturned_Data\Managed", fileName)))
                    {
                        Console.Write("outdated");
                        Console.WriteLine("Recreating Rocket Folder...");
                        FileInfo[] loaderFiles = new DirectoryInfo(loaderDir).GetFiles();
                        foreach (FileInfo file in loaderFiles)
                        {
                            file.Delete();
                        }
                        FileInfo[] originalFiles = new DirectoryInfo(@"Unturned_Data\Managed").GetFiles();
                        foreach (FileInfo file in originalFiles)
                        {
                            file.CopyTo(Path.Combine(loaderDir, file.Name), false);
                            if (checkFiles.Contains(file.Name))
                            {
                                file.CopyTo(Path.Combine(loaderDir, file.Name + ".original"), false);
                            }
                        }
                        break;
                    }
                    else {
                        Console.Write("up to date");
                    }
                }
            }
            else
            {
                Console.Write("does not exist");
                Directory.CreateDirectory(loaderDir);
                GrantAccess(loaderDir);

                FileInfo[] originalFiles = new DirectoryInfo(@"Unturned_Data\Managed").GetFiles();
                foreach (FileInfo file in originalFiles)
                {
                    file.CopyTo(Path.Combine(loaderDir, file.Name), false);
                }
            }

            if (!File.Exists(Path.Combine(loaderDir, "Rocket")))
            {

                Console.WriteLine("Downloading Rocket...");
                WebClient webClient = new WebClient();
                webClient.DownloadFile(apiUrl, Path.Combine(loaderDir, "RocketAPI.dll"));
                webClient.DownloadFile(loaderUrl, Path.Combine(loaderDir, "RocketLoader.exe"));

                Console.WriteLine("Patching Rocket...");

                Process patcher = new Process();
                patcher.StartInfo.FileName = Path.Combine(loaderDir, "RocketLoader.exe");
                patcher.StartInfo.WorkingDirectory = loaderDir;
                patcher.StartInfo.UseShellExecute = false;
                patcher.StartInfo.RedirectStandardOutput = true;
                patcher.StartInfo.CreateNoWindow = true;

                Console.ForegroundColor = ConsoleColor.Cyan;
                patcher.Start();
                while (!patcher.StandardOutput.EndOfStream)
                {
                    Console.WriteLine(patcher.StandardOutput.ReadLine());
                }
                patcher.WaitForExit();

                File.Create(Path.Combine(loaderDir, "Rocket"));

                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.Write("\nInitiating launch sequence");

            Directory.Move(@"Unturned_Data\Managed", @"Unturned_Data\Managed.original");
            Directory.Move(loaderDir, @"Unturned_Data\Managed");


            System.Threading.Thread.Sleep(500);

            System.Diagnostics.ProcessStartInfo launcher = new System.Diagnostics.ProcessStartInfo();
            launcher.FileName = "Unturned.exe";
            launcher.UseShellExecute = false;
            launcher.WorkingDirectory = Directory.GetCurrentDirectory();

            string arguments = String.Join(" ", args);
            if(!arguments.ToLower().Contains("secureserver")){
                arguments = "+secureserver/Rocket" + arguments;
            }

            launcher.Arguments = "-batchmode -nographics " + arguments ;

            Process launcherProcess = Process.Start(launcher);


            for (int i = 0; i < 6; i++)
            {
                Console.Write(".");
                System.Threading.Thread.Sleep(500);
            }

            Console.Write("\nRocket launch immediate");
            bool success = false;
            while (!success)
            {
                System.Threading.Thread.Sleep(500);

                try
                {
                    Directory.Move(@"Unturned_Data\Managed", loaderDir);
                    Directory.Move(@"Unturned_Data\Managed.original", @"Unturned_Data\Managed");
                    success = true;
                }
                catch (Exception)
                {

                }
            }

            launcherProcess.WaitForExit();
        }
        private static bool GrantAccess(string fullPath)
        {
            DirectoryInfo dInfo = new DirectoryInfo(fullPath);
            DirectorySecurity dSecurity = dInfo.GetAccessControl();
            dSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
            dInfo.SetAccessControl(dSecurity);
            return true;
        }

        static bool fileCompare(string file1, string file2)
        {
            int file1byte;
            int file2byte;
            FileStream fs1;
            FileStream fs2;

            // Determine if the same file was referenced two times.
            if (file1 == file2)
            {
                // Return true to indicate that the files are the same.
                return true;
            }

            if (!File.Exists(file1) || !File.Exists(file2)) return false;

            // Open the two files.
            fs1 = new FileStream(file1, FileMode.Open);
            fs2 = new FileStream(file2, FileMode.Open);

            // Check the file sizes. If they are not the same, the files 
            // are not the same.
            if (fs1.Length != fs2.Length)
            {
                // Close the file
                fs1.Close();
                fs2.Close();

                // Return false to indicate files are different
                return false;
            }

            // Read and compare a byte from each file until either a
            // non-matching set of bytes is found or until the end of
            // file1 is reached.
            do
            {
                // Read one byte from each file.
                file1byte = fs1.ReadByte();
                file2byte = fs2.ReadByte();
            }
            while ((file1byte == file2byte) && (file1byte != -1));

            // Close the files.
            fs1.Close();
            fs2.Close();

            // Return the success of the comparison. "file1byte" is 
            // equal to "file2byte" at this point only if the files are 
            // the same.
            return ((file1byte - file2byte) == 0);
        }
    }
}
