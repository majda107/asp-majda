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

            Console.WriteLine($"Starting server at port {server.Port}");
            server.Listen();
        }
    }
}
