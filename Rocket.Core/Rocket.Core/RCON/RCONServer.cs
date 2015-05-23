using Rocket.Core.Events;
using Rocket.Core.Logging;
using Rocket.Core.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace Rocket.Core.RCON
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

    public class RCONServer
    {
        private static Socket serverSocket;
        private static byte[] data = new byte[dataSize];
        private const int dataSize = 1024;
        private static Dictionary<Socket, Client> clientList = new Dictionary<Socket, Client>();

        public static void Listen(int port)
        {
            try
            {
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
                serverSocket.Listen(1);
                serverSocket.BeginAccept(new AsyncCallback(AcceptConnection), serverSocket);
                Logger.logRCON("Server started successfully at 0.0.0.0:" + port);
            }
            catch (Exception ex)
            {
                 Logger.logRCON("Error: " + ex.ToString());
            }
        }

        internal static void broadcast(string message)
        {
            foreach (Socket currentSocket in clientList.Keys.ToArray())
            {
                byte[] rmessage = Encoding.ASCII.GetBytes(message);
                currentSocket.BeginSend(rmessage, 0, 1, SocketFlags.None, new AsyncCallback(ReceiveData), currentSocket);
            }
        }

        private static void AcceptConnection(IAsyncResult result)
        {
            Socket oldSocket = (Socket)result.AsyncState;
            Socket newSocket = oldSocket.EndAccept(result);
            Client client = new Client((IPEndPoint)newSocket.RemoteEndPoint, DateTime.Now, EClientState.NotLogged);
            clientList.Add(newSocket, client);
            Logger.logRCON("Client logging in... (From: " + string.Format("{0}:{1}", client.remoteEndPoint.Address.ToString(), client.remoteEndPoint.Port) + ")");
            string output = "RocketRcon v" + Assembly.GetExecutingAssembly().GetName().Version;

            if (client.remoteEndPoint.Address == IPAddress.Loopback)
            {
                client.clientState = EClientState.LoggedIn;
            }
            else
            {
                output += "\r\nPassword: ";
                client.clientState = EClientState.Logging;
            }

            byte[] message = Encoding.ASCII.GetBytes(output);
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
                //quit
                if (received == 0)
                {
                    clientSocket.Close();
                    clientList.Remove(clientSocket);
                    serverSocket.BeginAccept(new AsyncCallback(AcceptConnection), serverSocket);
                    Logger.logRCON("Client disconnected. (" + string.Format("{0}:{1}", client.remoteEndPoint.Address.ToString(), client.remoteEndPoint.Port) + ")");
                    return;
                }
                //quit message
                if (data[0] == 0x2E && data[1] == 0x0D && client.commandIssued.Length == 0)
                {
                    string currentCommand = client.commandIssued;
                    Logger.logRCON(string.Format("Received '{0}' while EClientStatus '{1}' ({2}:{3})", currentCommand, client.clientState.ToString(), client.remoteEndPoint.Address.ToString(), client.remoteEndPoint.Port));
                    client.commandIssued = "";
                    handleCommand(clientSocket, currentCommand);
                }
                //client welcome header (putty)
                else if (data[0] == 0xFF && data[1] == 0xFB)
                {
                    client.commandIssued = "";
                    clientSocket.BeginReceive(data, 0, dataSize, SocketFlags.None, new AsyncCallback(ReceiveData), clientSocket);
                }
                else if (data[0] == 0x0D && data[1] == 0x0A)
                {
                    string currentCommand = client.commandIssued;
                    if (!String.IsNullOrEmpty(currentCommand))
                    {
                        Logger.logRCON(string.Format("Received '{0}' ({1}:{2})", currentCommand, client.remoteEndPoint.Address.ToString(), client.remoteEndPoint.Port));
                        handleCommand(clientSocket, currentCommand);
                    }
                    client.commandIssued = "";
                }
                // 0x08 => remove character
                else if (data[0] == 0x08)
                {
                    if (client.commandIssued.Length > 0)
                    {
                        client.commandIssued = client.commandIssued.Substring(0, client.commandIssued.Length - 1);
                        byte[] message = Encoding.ASCII.GetBytes("\u0020\u0008");
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
                    client.commandIssued += Encoding.ASCII.GetString(data, 0, received);
                    clientSocket.BeginReceive(data, 0, dataSize, SocketFlags.None, new AsyncCallback(ReceiveData), clientSocket);
                }
            }
            catch (ObjectDisposedException)
            {
                Logger.logRCON(string.Format("Client quit ({1}:{2})", client.remoteEndPoint.Address.ToString(), client.remoteEndPoint.Port));
            }
            catch (SocketException)
            {
                Logger.logRCON(string.Format("Client quit ({1}:{2})", client.remoteEndPoint.Address.ToString(), client.remoteEndPoint.Port));
            }
            catch (Exception ex)
            {
                 Logger.LogException(ex);
            }
        }

        private static void handleCommand(Socket clientSocket, string Input)
        {
            string answer = "";
            byte[] dataInput = Encoding.ASCII.GetBytes(Input);
            Client client;
            clientList.TryGetValue(clientSocket, out client);
            if (client == null) return;
            if (client.clientState == EClientState.LoggedIn)
            {
                try
                {
                    string cmd = Input.Split(' ')[0].ToLower();
                    if (RocketEvents.triggerOnRocketCommandTriggered(Input))
                    {
                        answer += "Command executed";
                    }
                    else if (Input == "exit")
                    {
                        clientSocket.Close();
                        clientList.Remove(clientSocket);
                        serverSocket.BeginAccept(new AsyncCallback(AcceptConnection), serverSocket);
                        Logger.logRCON("Client disconnected. (" + string.Format("{0}:{1}", client.remoteEndPoint.Address.ToString(), client.remoteEndPoint.Port) + ")");
                        return;
                    }
                    else
                    {
                        answer += "Command not found";
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex);
                    answer += "An exception was thrown on the server: " + ex.Message;
                }
            }
            if (client.clientState == EClientState.Logging)
            {
                if (Input == RocketSettingsManager.Settings.RCON.Password)
                {
                    Logger.logRCON("Client has logged in");
                    client.clientState = EClientState.LoggedIn;
                    answer += "Successfully logged in.";
                }
                else
                {
                    clientSocket.Close();
                    clientList.Remove(clientSocket);
                    serverSocket.BeginAccept(new AsyncCallback(AcceptConnection), serverSocket);
                    Logger.logRCON("Client kicked because of incorrect password. (" + string.Format("{0}:{1}", client.remoteEndPoint.Address.ToString(), client.remoteEndPoint.Port) + ")");
                    return;
                }
            }
            byte[] message = Encoding.ASCII.GetBytes(answer + "\r\n > ");
            clientSocket.BeginSend(message, 0, message.Length, SocketFlags.None, new AsyncCallback(SendData), clientSocket);
        }

    }
}