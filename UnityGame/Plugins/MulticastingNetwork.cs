using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace UnityGame.Plugins
{
    public class MulticastingNetwork : INetwork
    {
        private const string MulticastAddress = "230.0.0.1";
        private const int MulticastPort = 11000;
        private Socket socket;
        private CancellationTokenSource cts = new CancellationTokenSource();

        public event Action<string> OnMessageReceived;

        public MulticastingNetwork()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        }

        public void SendMessage(string message)
        {
            try
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(MulticastAddress), MulticastPort);
                socket.SendTo(Encoding.ASCII.GetBytes(message), endPoint);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error when sending message: " + e);
            }
        }

        public Task StartListening()
        {
            return Task.Run(() => ReceiveMessage(cts.Token));
        }

        private void ReceiveMessage(CancellationToken token)
        {
            try
            {
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                socket.Bind(new IPEndPoint(IPAddress.Any, MulticastPort));

                socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership,
                    new MulticastOption(IPAddress.Parse(MulticastAddress)));

                byte[] buffer = new byte[1024];
                while (!token.IsCancellationRequested)
                {
                    EndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
                    int receivedBytes = socket.ReceiveFrom(buffer, ref remoteEP);
                    string receivedMessage = Encoding.ASCII.GetString(buffer, 0, receivedBytes);
                    
                    OnMessageReceived?.Invoke(receivedMessage);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error when trying to receive message: " + e);
            }
        }

        public void StopListening()
        {
            cts.Cancel();
            socket.Close();
        }
    }
}
