using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rocket.Unturned
{
    using System;
    using Rocket.Unturned.Plugins;
    using Rocket.Unturned.Logging;
    using System.IO;
    using System.Diagnostics;
    using System.Threading;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using System.ComponentModel;
    using SDG;

    public class RocketConsole : MonoBehaviour
    {
        FileStream fileStream = null;
        private void Awake()
        {
            try
            {
                fileStream = new FileStream("console", FileMode.Create);

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
                x = Console.ReadLine();
                if (Steam.ConsoleInput.onInputText != null && x.Trim().Length != 0) Steam.ConsoleInput.onInputText(x);
            }
            while (true);
        }
    }


}
