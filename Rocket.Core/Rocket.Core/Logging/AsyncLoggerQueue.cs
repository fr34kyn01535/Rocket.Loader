using Rocket.Core.RCON;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
//Source: http://stackoverflow.com/questions/1181561/how-to-effectively-log-asynchronously
namespace Rocket.Core.Logging
{
    public class LogEntry
    {
        public ELogType Severity;
        public string Message;
    }

    public class AsyncLoggerQueue
    {
        public static AsyncLoggerQueue Current = new AsyncLoggerQueue();

        private static readonly object logEntryQueueLock = new object();

        private Queue<LogEntry> _LogEntryQueue = new Queue<LogEntry>();
        private BackgroundWorker _Logger = new BackgroundWorker();

        private AsyncLoggerQueue()
        {
            //configure background worker
            _Logger.WorkerSupportsCancellation = false;
            _Logger.DoWork += new DoWorkEventHandler(_Logger_DoWork);
        }

        public void Enqueue(LogEntry le)
        {
            //lock during write
            lock (logEntryQueueLock)
            {
                _LogEntryQueue.Enqueue(le);

                //while locked check to see if the BW is running, if not start it
                if (!_Logger.IsBusy)
                    _Logger.RunWorkerAsync();
            }
        }

        private void _Logger_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                LogEntry le = null;

                bool skipEmptyCheck = false;
                lock (logEntryQueueLock)
                {
                    if (_LogEntryQueue.Count <= 0) //if queue is empty than BW is done
                        return;
                    else if (_LogEntryQueue.Count > 1) //if greater than 1 we can skip checking to see if anything has been enqueued during the logging operation
                        skipEmptyCheck = true;

                    //dequeue the LogEntry that will be written to the log
                    le = _LogEntryQueue.Dequeue();
                }

                processLog(le);

                if (skipEmptyCheck) //if LogEntryQueue.Count was > 1 before we wrote the last LogEntry we know to continue without double checking
                {
                    lock (logEntryQueueLock)
                    {
                        if (_LogEntryQueue.Count <= 0) //if queue is still empty than BW is done
                            return;
                    }
                }
            }
        }
        private void processLog(LogEntry entry) {
            if (String.IsNullOrEmpty(Rocket.HomeFolder) || !Directory.Exists(Rocket.HomeFolder + "Logs/")) return;
            StreamWriter streamWriter = File.AppendText(Rocket.HomeFolder + "Logs/" + "Rocket.log");
            streamWriter.WriteLine("[" + DateTime.Now + "] [" + entry.Severity.ToString() + "] " + entry.Message);
            streamWriter.Close();
            if (SettingsManager.Settings.RCON.Port > 1) 
                RCONServer.broadcast(entry.Message);
        }
    }
}
