using System;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.Reflection;
using Rocket.Core.Logging;
using Rocket.Core.Events;

namespace Rocket.Core.RCON
{
    public class RCONServer
    {
        private static List<RCONConnection> clients = new List<RCONConnection>();
        private TcpListener listener;
        private bool exiting = false;
        private static string password = "";
        internal static RCONServer Instance;


        internal static void Listen(short port, string password)
        {
            Instance = new RCONServer(port, password);
        }

        public RCONServer(short port, string password)
        {
            RCONServer.password = password;
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();

            Logger.Log("Waiting for new connection...");

            while (!exiting)
            {
                RCONConnection newclient = new RCONConnection(listener.AcceptTcpClient());
                clients.Add(newclient);
                newclient.Send("RocketRcon v" + Assembly.GetExecutingAssembly().GetName().Version + "\r\n");
                ThreadPool.QueueUserWorkItem(handleConnection, newclient);
            }
        }

        private static void handleConnection(object obj)
        {
            RCONConnection newclient = (RCONConnection)obj;
            string command = "";
            while (newclient.Client.Client.Connected)
            {
                Thread.Sleep(100);
                command = newclient.Read();
                if (command == "") break;
                command = command.TrimEnd('\n', '\r', ' ');
                if (command == "quit") break;
                if (command == "ia")
                {
                    newclient.Send("Toggled interactive mode");
                    newclient.Interactive = !newclient.Interactive;
                }
                if (command == "") continue;
                if (command == "login")
                {
                    if (newclient.Authenticated)
                        newclient.Send("Notice: You are already logged in!\r\n");
                    else
                        newclient.Send("Syntax: login <password>");
                    continue;
                }
                if (command.Split(' ').Length > 1 && command.Split(' ')[0] == "login")
                {
                    if (newclient.Authenticated)
                    {
                        newclient.Send("Notice: You are already logged in!\r\n");
                        continue;
                    }
                    else
                    {
                        if (command.Split(' ')[1] == password)
                        {
                            newclient.Authenticated = true;
                            newclient.Send("Success: You have logged in!\r\n");
                            Logger.Log("Client has logged in!");
                            continue;
                        }
                        else
                        {
                            newclient.Send("Error: Invalid password!\r\n");
                            Logger.Log("Client has failed to log in.");
                            break;
                        }
                    }
                }

                if (command == "set")
                {
                    newclient.Send("Syntax: set [option] [value]");
                    continue;
                }
                if (!newclient.Authenticated)
                {
                    newclient.Send("Error: You have not logged in yet!\r\n");
                    continue;
                }
                Logger.Log("Client has executed command \"" + command + "\"");
                RocketEvents.triggerOnRocketCommandTriggered(command);
                command = "";
            }

            clients.Remove(newclient);
            newclient.Send("Good bye!");
            Thread.Sleep(1500);
            Logger.Log("Client has disconnected! (IP: " + newclient.Client.Client.RemoteEndPoint + ")");
            newclient.Close();
        }

        public static void Broadcast(string message)
        {
            foreach (RCONConnection client in clients)
            {
                if (client.Authenticated)
                    client.Send(message);
            }
        }

        ~RCONServer()
        {
            exiting = true;
            foreach (RCONConnection leclient in clients)
            {
                leclient.Send("Good bye!");
                Thread.Sleep(150);
                leclient.Close();
            }
            clients.Clear();
            this.listener.Stop();
        }

        public static string Read(TcpClient client)
        {
            byte[] _data = new byte[1];
            string data = "";
            NetworkStream _stream = client.GetStream();

            while (true)
            {
                try
                {
                    int k = _stream.Read(_data, 0, 1);
                    if (k == 0)
                        return "";
                    char kk = Convert.ToChar(_data[0]);
                    data += kk;
                    if (kk == '\n')
                        break;
                }
                catch
                {
                    return "";
                }
            }
            return data;
        }

        public static void Send(TcpClient client, string text)
        {
            byte[] data = new UTF8Encoding().GetBytes(text);
            client.GetStream().Write(data, 0, text.Length);
        }
    }
}
