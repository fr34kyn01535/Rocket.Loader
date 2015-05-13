using Rocket.Components;
using Rocket.Logging;
using SDG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace Rocket.Rcon
{
    public enum EClientState
    {
        NotLogged = 0,
        Logging = 1,
        LoggedIn = 2
    }

    public class Client
    {
        public IPEndPoint remoteEndPoint;
        public DateTime connectedAt;
        public EClientState clientState;
        public string commandIssued = string.Empty;

        public Client(IPEndPoint _remoteEndPoint, DateTime _connectedAt, EClientState _clientState)
        {
            this.remoteEndPoint = _remoteEndPoint;
            this.connectedAt = _connectedAt;
            this.clientState = _clientState;
        }
    }

    public class RocketRconServer
    {
        private static Socket serverSocket;
        private static byte[] data = new byte[dataSize];
        private static bool newClients = true;
        private const int dataSize = 1024;
        private static Dictionary<Socket, Client> clientList = new Dictionary<Socket, Client>();

        private static Dictionary<IntPtr, List<String>> logList = new Dictionary<IntPtr, List<String>>();

        public static void Listen(int port)
        {
            try
            {
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port);
                serverSocket.Bind(endPoint);
                serverSocket.Listen(0);
                serverSocket.BeginAccept(new AsyncCallback(AcceptConnection), serverSocket);
                log("Server started successfully at 0.0.0.0:" + port);
            }
            catch (Exception ex)
            {
                log("Error: " + ex.ToString());
            }
        }

        internal static void broadcast(string message)
        {
            foreach (List<string> h in logList.Values)
            {
                h.Add(message.Trim() + "\r\n");
            }
            foreach (Socket currentSocket in clientList.Keys.ToArray())
            {
                currentSocket.BeginSend(new byte[1] { 1 }, 0, 1, SocketFlags.None, new AsyncCallback(ReceiveData), currentSocket);
            }
        }

        private static void log(string m)
        {
            Logger.logRcon(m);
        }

        private static void AcceptConnection(IAsyncResult result)
        {
            if (!newClients) return;
            Socket oldSocket = (Socket)result.AsyncState;
            Socket newSocket = oldSocket.EndAccept(result);
            Client client = new Client((IPEndPoint)newSocket.RemoteEndPoint, DateTime.Now, EClientState.NotLogged);
            clientList.Add(newSocket, client);
            logList.Add(newSocket.Handle, new List<string>());
            log("Client logging in... (From: " + string.Format("{0}:{1}", client.remoteEndPoint.Address.ToString(), client.remoteEndPoint.Port) + ")");
            string output = "Password: ";
            client.clientState = EClientState.Logging;
            byte[] message = Encoding.UTF8.GetBytes(output);
            newSocket.BeginSend(message, 0, message.Length, SocketFlags.None, new AsyncCallback(SendData), newSocket);
            serverSocket.BeginAccept(new AsyncCallback(AcceptConnection), serverSocket);
        }

        private static void SendData(IAsyncResult result)
        {
            try
            {
                Socket clientSocket = (Socket)result.AsyncState;
                clientSocket.EndSend(result);
                clientSocket.BeginReceive(data, 0, dataSize, SocketFlags.None, new AsyncCallback(ReceiveData), clientSocket);
            }
            catch { }
        }

        private static void ReceiveData(IAsyncResult result)
        {
            Client client;
            Socket clientSocket = (Socket)result.AsyncState;
            clientList.TryGetValue(clientSocket, out client);
            try
            {
                int received = clientSocket.EndReceive(result);
                if (received == 0)
                {
                    logList.Remove(clientSocket.Handle);
                    clientSocket.Close();
                    clientList.Remove(clientSocket);
                    serverSocket.BeginAccept(new AsyncCallback(AcceptConnection), serverSocket);
                    log("Client disconnected. (" + string.Format("{0}:{1}", client.remoteEndPoint.Address.ToString(), client.remoteEndPoint.Port) + ")");
                    return;
                }

                if (data[0] == 0x2E && data[1] == 0x0D && client.commandIssued.Length == 0)
                {
                    string currentCommand = client.commandIssued;
                    log(string.Format("Received '{0}' while EClientStatus '{1}' ({2}:{3})", currentCommand, client.clientState.ToString(), client.remoteEndPoint.Address.ToString(), client.remoteEndPoint.Port));
                    client.commandIssued = "";
                    handleCommand(clientSocket, currentCommand);
                }
                else if (data[0] == 0x0D && data[1] == 0x0A)
                {
                    string currentCommand = client.commandIssued;
                    if (!String.IsNullOrEmpty(currentCommand))
                        log(string.Format("Received '{0}' ({1}:{2})", currentCommand, client.remoteEndPoint.Address.ToString(), client.remoteEndPoint.Port));
                    client.commandIssued = "";
                    handleCommand(clientSocket, currentCommand);
                }
                else
                {
                    // 0x08 => remove character
                    if (data[0] == 0x08)
                    {
                        if (client.commandIssued.Length > 0)
                        {
                            client.commandIssued = client.commandIssued.Substring(0, client.commandIssued.Length - 1);
                            byte[] message = Encoding.UTF8.GetBytes("\u0020\u0008");
                            clientSocket.BeginSend(message, 0, message.Length, SocketFlags.None, new AsyncCallback(SendData), clientSocket);
                        }
                        else
                        {
                            clientSocket.BeginReceive(data, 0, dataSize, SocketFlags.None, new AsyncCallback(ReceiveData), clientSocket);
                        }
                    }
                    // 0x7F => delete character
                    else if (data[0] == 0x7F)
                    {
                        clientSocket.BeginReceive(data, 0, dataSize, SocketFlags.None, new AsyncCallback(ReceiveData), clientSocket);
                    }
                    else
                    {
                        string currentCommand = client.commandIssued;
                        client.commandIssued += Encoding.UTF8.GetString(data, 0, received);
                        clientSocket.BeginReceive(data, 0, dataSize, SocketFlags.None, new AsyncCallback(ReceiveData), clientSocket);
                    }
                }
            }
            catch (ObjectDisposedException ex)
            {
                log(string.Format("Client quit ({1}:{2})", client.remoteEndPoint.Address.ToString(), client.remoteEndPoint.Port));
            }
            catch (SocketException ex)
            {
                log(string.Format("Client quit ({1}:{2})", client.remoteEndPoint.Address.ToString(), client.remoteEndPoint.Port));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        private static void handleCommand(Socket clientSocket, string Input)
        {
            List<string> h = logList[clientSocket.Handle];
            if (h.Count > 23)
            {
                int l = h.Count - 23;
                for (int i = 0; i < l; i++)
                {
                    h.RemoveAt(0);
                }
            }

            string Output = "RocketRcon v" + Assembly.GetExecutingAssembly().GetName().Version + "\r\n";
            byte[] dataInput = Encoding.UTF8.GetBytes(Input);
            Client client;
            clientList.TryGetValue(clientSocket, out client);

            if (client.clientState == EClientState.LoggedIn)
            {
                try
                {
                    //h.Add(Input + "\n\r");
                    string cmd = Input.Split(' ')[0].ToLower();
                    if (Commander.execute(new Steamworks.CSteamID(0),Input))
                    {
                        //Executed
                    }
                    else if (Input == "exit")
                    {
                        logList.Remove(clientSocket.Handle);
                        clientSocket.Close();
                        clientList.Remove(clientSocket);
                        serverSocket.BeginAccept(new AsyncCallback(AcceptConnection), serverSocket);
                        log("Client disconnected. (" + string.Format("{0}:{1}", client.remoteEndPoint.Address.ToString(), client.remoteEndPoint.Port) + ")");
                        return;
                    }
                    else if (Input == "cls")
                    {
                        h.Clear();
                    }
                    else
                    {
                        // h.Add("Command unknown\n\r");
                    }

                    foreach (string entry in h)
                    {
                        Output += entry;
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex);
                    h.Add("An exception was thrown on the server: " + ex.Message + "\r\n");
                }
            }
            if (client.clientState == EClientState.Logging)
            {
                if (Input == RocketSettings.RconPassword)
                {
                    log("Client has logged in");
                    client.clientState = EClientState.LoggedIn;
                }
                else
                {
                    logList.Remove(clientSocket.Handle);
                    clientSocket.Close();
                    clientList.Remove(clientSocket);
                    serverSocket.BeginAccept(new AsyncCallback(AcceptConnection), serverSocket);
                    log("Client kicked because of incorrect password. (" + string.Format("{0}:{1}", client.remoteEndPoint.Address.ToString(), client.remoteEndPoint.Port) + ")");
                    return;
                }
            }
            byte[] message = Encoding.UTF8.GetBytes("\u001B[1J\u001B[H" + Output);
            clientSocket.BeginSend(message, 0, message.Length, SocketFlags.None, new AsyncCallback(SendData), clientSocket);
        }

    }
}