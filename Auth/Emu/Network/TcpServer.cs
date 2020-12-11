//using Auth.Emu.Network.Packets.SEND;
using Auth.Emu.Network.Packets.SEND;
using Core.Utils;
using System;
using System.Net;
using System.Net.Sockets;

namespace Auth.Emu.Network
{
    public class TcpServer
    {
        private readonly Socket _listenSocket;
        private string _host;
        private int _port;
        public TcpServer(string host, int port)
        {
            _host = host;
            _port = port;
            _listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Listen()
        {
            try
            {
                _listenSocket.Bind(new IPEndPoint(IPAddress.Parse(_host), _port));
                _listenSocket.Listen(100);

                Logging.InfoTT("Tcp server started at {0}:{1}", _host, _port);

                _listenSocket.BeginAccept(OnNewConnection, null);
            }
            catch (Exception)
            {
                Logging.Alert($"Failed to bind the socket to port: {_port}");
            }
        }

        private void OnNewConnection(IAsyncResult ar)
        {
            Socket newSocket = null;
            try
            {
                newSocket = _listenSocket.EndAccept(ar);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Connection Accept Exception:");
                Console.WriteLine(ex.ToString());
            }

            if (newSocket != null)
            {
                Logging.UserB($"New connection from: {newSocket.RemoteEndPoint}");

                var client = new ClientConnection(newSocket);

                // Send connection message.
                client.Send(new SMSetFrameworkInformation
                {
                    FrameWorkData = client.BDOCrypt.GetFrameWorkData()
                });
            }

            if (_listenSocket.IsBound)
            {
                // Start listening for more connections!
                _listenSocket.BeginAccept(OnNewConnection, null);
            }
        }
    }
}