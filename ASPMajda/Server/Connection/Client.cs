using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace ASPMajda.Server.Connection
{
    class Client
    {
        public TcpClient TcpClient { get; private set; }
        public NetworkStream Stream { get; private set; }
        public Client(TcpClient client)
        {
            this.TcpClient = client;
            this.Stream = client.GetStream();
        }
    }
}
