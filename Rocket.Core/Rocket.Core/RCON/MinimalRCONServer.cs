using Rocket.Core.Events;
using Rocket.Core.Logging;
using Rocket.Core.Settings;
using Rocket.Core.Tasks;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Rocket.Core.RCON
{
    public class StateObject
    {
        public Socket workSocket = null;
        public const int BufferSize = 1024;
        public byte[] buffer = new byte[BufferSize];
        public StringBuilder sb = new StringBuilder();
    }

    public class MinimalRconServer
    {
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        private static int _port;

        public static void Listen(int port)
        {
            _port = port;
            MinimalRconServer server = new MinimalRconServer();
            Thread workerThread = new Thread(server.DoWork);
            workerThread.Start();
        }

        private volatile bool _shouldStop;
        public void RequestStop()
        {
            _shouldStop = true;
        }

        public void DoWork()
        {
            byte[] bytes = new Byte[1024];
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                listener.Bind(new IPEndPoint(IPAddress.Any, _port));
                listener.Listen(100);
                RocketTaskManager.Enqueue(() => { Logger.logRCON("Minimal server started successfully at 0.0.0.0:" + _port); });
                

                while (!_shouldStop)
                {
                    allDone.Reset();
                    listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);
                    allDone.WaitOne();
                }
            }
            catch (Exception e)
            {
                RocketTaskManager.Enqueue(() => { Logger.logRCON(e.ToString()); });
            }
        }

        public static void AcceptCallback(IAsyncResult ar)
        {
            allDone.Set();
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            StateObject state = new StateObject();
            state.workSocket = handler;
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
        }

        public static void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty;

            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;

            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
                content = state.sb.ToString();

                if (!content.StartsWith(RocketSettingsManager.Settings.RCON.Password + "|")) { Send(handler, "false"); return; }

                content = content.Replace(RocketSettingsManager.Settings.RCON.Password + "|", "");

                //if (content.IndexOf("<EOF>") > -1)
                //{
                //    content = content.Replace("<EOF>", "");
                    IPEndPoint remoteIpEndPoint = state.workSocket.RemoteEndPoint as IPEndPoint;
                    bool success = false;
                    RocketTaskManager.Enqueue(() =>
                    {
                        Logger.logRCON(string.Format("Received '{0}' (From: " + string.Format("{0}:{1}", remoteIpEndPoint.Address.ToString(), remoteIpEndPoint.Port) + ")", content));
                        success = RocketEvents.triggerOnRocketCommandTriggered(content); 
                    });

                    if (success)
                    {
                        Send(handler, "true");
                    }
                    else
                    {
                        Send(handler, "false");
                    }

                //}
                //else
                //{
                //    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                //}
            }
        }

        private static void Send(Socket handler, String data)
        {
            byte[] byteData = Encoding.ASCII.GetBytes(data);
            handler.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), handler);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket handler = (Socket)ar.AsyncState;

                int bytesSent = handler.EndSend(ar);
                //Console.WriteLine("Sent {0} bytes to client.", bytesSent);

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
            catch (Exception e)
            {
                RocketTaskManager.Enqueue(() => { Logger.logRCON(e.ToString()); });
            }
        }
    }
}