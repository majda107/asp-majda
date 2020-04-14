using ASPMajda.Server;
using System;
using System.Net;

namespace ASPMajda
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new HttpServer(IPAddress.Any, 6969);
            server.Listen();
        }
    }
}
