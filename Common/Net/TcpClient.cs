
using SimpleTcp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ParticipantConsole.Net
{
    public class TcpClient
    {

        public event EventHandler<byte[]> DataReceived;
        public event EventHandler Connected;
        public event EventHandler Disconnected;

        SimpleTcp.TcpClient client;

        public TcpClient(int serverPort, string serverIp = null)
        {
            client = new SimpleTcp.TcpClient(serverPort, false, null, null, serverIp);

            client.ConnectTimeoutSeconds = 200;

            client.DataReceived += OnDataReceived;
            client.Connected += OnConnected;
            client.Disconnected += OnDisconnected;
        }

        public void Dispose()
        {
            client.DataReceived -= OnDataReceived;
            client.Connected -= OnConnected;
            client.Disconnected -= OnDisconnected;

            client.Dispose();
        }

        public void Connect()
        {
            try
            { 
                client.Connect();

            }

            catch (Exception ex)
            {

            }

        }

        public void Send(byte[] data)
        {
            if (!client.IsConnected)
                return;

            client.Send(data);
        }

        public void Send(string data)
        {
            if (!client.IsConnected)
                return;

            client.Send(data);
        }


        public string ServerIp
        {
            get => client.ServerIp;

            set => client.ServerIp = value;
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

        void OnDataReceived(object sender, DataReceivedFromServerEventArgs e)
        {
            DataReceived(sender, e.Data);
        }

        void OnConnected(object sender, EventArgs e)
        {
            Connected(sender, e);
        }

        void OnDisconnected(object sender, EventArgs e)
        {
            Disconnected(sender, e);
        }

    }
}
