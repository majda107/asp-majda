using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ASPMajda.Server.Engine
{
    class ListenerPool
    {
        public Dictionary<Thread, TcpListener> Listeners { get; private set; }

        public ListenerPool()
        {
            this.Listeners = new Dictionary<Thread, TcpListener>();
        }

        public void Add(Thread thread, TcpListener listener)
        {
            this.Listeners.Add(thread, listener);
        }
    }
}
