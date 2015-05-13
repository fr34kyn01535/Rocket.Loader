using RCONTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Rocket.RCON
{
    public class StateObject
    {
        public Socket workSocket = null;
        public const int BufferSize = 1024;
        public byte[] buffer = new byte[BufferSize];
        public StringBuilder sb = new StringBuilder();
    }

    public class MinimalRocketRconServer
    {
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        private static int _port;

        public static void Listen(int port)
        {
            _port = port;
            MinimalRocketRconServer server = new MinimalRocketRconServer();
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
                while (!_shouldStop)
                {
                    allDone.Reset();
                    listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);
                    allDone.WaitOne();
                }
            }
            catch (Exception e)
            {
                Logger.logRCON(e.ToString());
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

                if (!content.Contains("|")) Send(handler, "error");



                //if (content.IndexOf("<EOF>") > -1)
                //{
                //    content = content.Replace("<EOF>", "");
                    IPEndPoint remoteIpEndPoint = state.workSocket.RemoteEndPoint as IPEndPoint;
                    Logger.logRCON(string.Format("Received '{0}' (From: " + string.Format("{0}:{1}", remoteIpEndPoint.Address.ToString(), remoteIpEndPoint.Port) + ")", content));

                    if (Commander.execute(new Steamworks.CSteamID(0), content))
                    {
                        Send(handler, "ok");
                    }
                    else
                    {
                        Send(handler, "error");
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
                Console.WriteLine(e.ToString());
            }
        }
    }
}