using SimpleTcp;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Ensemble.Net
{
    public class TcpServer : IDisposable
    {

        public event EventHandler<string> ClientConnected;

        public event EventHandler<string> ClientDisconnected;

        public event EventHandler<byte[]> DataReceived;

        SimpleTcp.TcpServer server;

        const int ServerPort = 8082;


        public TcpServer()
        {

        }

        public string LocalIpAddress
        {
            get
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
                return string.Empty;
            }
        }

        public void Disconnect(string ipPort)
        {
            server.DisconnectClient(ipPort);
        }

        public void Send(string ipPort, byte[] data)
        {
            server.Send(ipPort, data);
        }

        public void Send(string ipPort, string data)
        {
            server.Send(ipPort, data);
        }

        public void Dispose()
        {
            server.Dispose();
            GC.SuppressFinalize(this);
        }

        public void Start()
        {
            server = new SimpleTcp.TcpServer("0.0.0.0", ServerPort, false, null, null);
            server.IdleClientTimeoutSeconds = 0;
            // set callbacks
            server.ClientConnected += OnClientConnected;
            server.DataReceived += OnDataReceived;
            server.ClientDisconnected += OnClientDisconnected;
            server.Start();
        }

        void OnClientConnected(object sender, ClientConnectedEventArgs e)
        {
            ClientConnected(sender, e.IpPort);

        }

        void OnClientDisconnected(object sender, ClientDisconnectedEventArgs e)
        {
            ClientDisconnected(sender, e.IpPort);
        }

        void OnDataReceived(object sender, DataReceivedFromClientEventArgs e)
        {
            DataReceived(sender,e.Data);
        }
    }
}
