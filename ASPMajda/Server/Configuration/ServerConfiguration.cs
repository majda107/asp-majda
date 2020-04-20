using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ASPMajda.Server.Configuration
{
    class ServerConfiguration : Configuration, IServerConfiguration
    {
        public IPAddress Address { get; private set; }
        public int Port { get; private set; }
        public ServerConfiguration(IPAddress address, int port)
        {
            this.Address = address;
            this.Port = port;
        }
        public IPAddress GetAddress()
        {
            return this.Address;
        }

        public int GetPort()
        {
            return this.Port;
        }
    }
}
