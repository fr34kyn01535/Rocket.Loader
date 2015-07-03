using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rocket.Unturned
{
    using System;
    using System.IO;
    using System.Diagnostics;
    using System.Threading;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using System.ComponentModel;
    using Rocket.Unturned.Logging;
    using SDG.Unturned;

    public class RocketConsole : MonoBehaviour
    {
        FileStream fileStream = null;
        private void Awake()
        {
            try
            {
                fileStream = new FileStream(Steam.InstanceName+".console", FileMode.Create,FileAccess.Write,FileShare.ReadWrite);

                StreamWriter streamWriter = new StreamWriter(fileStream, System.Text.Encoding.ASCII)
                {
                    AutoFlush = true
                };

                Console.SetOut(streamWriter);

                readingThread = new Thread(new ThreadStart(DoRead));
                readingThread.Start();
            }
            catch (Exception ex)
            {
                Logger.Log("Error: " + ex.ToString());
            }
        }

        private void Destroy() {
            if (fileStream != null)
            {
                fileStream.Close();
                fileStream.Dispose();
            }
        }

        private static Thread readingThread;

        private static void DoRead()
        {
            string x;
            do
            {
                try
                {
                x = Console.ReadLine();
                if (x != null && Steam.ConsoleInput != null && Steam.ConsoleInput.onInputText != null && x.Trim().Length != 0) Steam.ConsoleInput.onInputText(x);

                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }
            }
            while (true);
        }
    }


}
