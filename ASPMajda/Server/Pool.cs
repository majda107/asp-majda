using ASPMajda.Server.Connection;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ASPMajda.Server
{
    class Pool
    {
        public Dictionary<Client, Thread> Threads { get; private set; }
        public object Lock { get; private set; }
        public Pool()
        {
            this.Threads = new Dictionary<Client, Thread>();
            this.Lock = new object();
        }

        public void Add(Client client, Thread thread)
        {
            lock(this.Lock)
            {
                this.Threads.Add(client, thread);
            }
        }

        public void Remove(Client client)
        {
            lock(this.Lock)
            {
                this.Threads.Remove(client);
            }
        }
    }
}
