using ASPMajda.Server.Connection;
using ASPMajda.Server.Controller;
using ASPMajda.Server.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ASPMajda.Server
{
    class HttpServer
    {
        public Pool Pool { get; private set; }
        public IList<IControllerHandler> ControllerHandlers { get; private set; }
        public IPAddress Address { get; private set; }
        public int Port { get; private set; }

        private TcpListener listener;
        public HttpServer(IPAddress address, int port)
        {
            this.Address = address;
            this.Port = port;

            this.listener = new TcpListener(address, port);

            this.Pool = new Pool();
            this.ControllerHandlers = new List<IControllerHandler>();

            this.Init();
        }

        private void Init()
        {
            var custom = new ControllerHandler();
            custom.Register(new ControllerAction(Method.GET, "/test", (body) =>
            {
                Console.WriteLine("HAHAHA WORKS!");
                return ResponseMessage.Error;
            }));

            custom.Register(new ControllerAction(Method.POST, "/testpost", (body) =>
            {
                Console.WriteLine("POST WORKING!");
                Console.WriteLine(body);
                return ResponseMessage.Error;
            }));

            this.ControllerHandlers.Add(new FileControllerHandler(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
            this.ControllerHandlers.Add(custom);
        }

        public void Listen()
        {
            this.listener.Start();
            while (true)
            {
                var client = new Client(listener.AcceptTcpClient());

                var thread = new Thread(() =>
                {
                    this.ProcessClient(client);
                });

                this.Pool.Add(client, thread);
                thread.Start(); // LAUNCH THREAD!
            }
        }

        private void ProcessClient(Client client)
        {
            using (var sr = new StreamReader(client.Stream))
            using (var sw = new StreamWriter(client.Stream))
            {
                var message = this.HandleRequest(sr);
                if (message.HasBody)
                    this.HandleBody(sr, ref message);

                ResponseMessage response = ResponseMessage.Error;
                foreach (var handler in this.ControllerHandlers)
                    if (handler.TryFire(message, out response)) break;

                this.HandleResponse(sw, response);
                client.Stream.Close();
            }

            this.Pool.Remove(client);
        }

        private void HandleBody(StreamReader sr, ref RequestMessage message)
        {
            int len = int.Parse(message.Headers["Content-Length"]);
            char[] buffer = new char[len];
            sr.Read(buffer, 0, len);
            message.Body = new string(buffer);
        }

        private void HandleResponse(StreamWriter sw, ResponseMessage response)
        {
            if (response == null) response = ResponseMessage.Error;

            sw.WriteLine($"HTTP/1.0 ${response.StatusCode} OK");
            foreach (var header in response.Headers)
                sw.WriteLine($"{header.Key}: {header.Value}");

            if (response.Content != null)
            {
                sw.WriteLine();
                sw.Flush();
                
                var s = response.Content.GetStream();
                s.CopyTo(sw.BaseStream);
                s.Dispose();
            }
        }

        private RequestMessage HandleRequest(StreamReader sr)
        {
            var message = new RequestMessage();
            //using (var sr = new StreamReader(stream))
            //{
            var line = sr.ReadLine();
            message.ParsePath(line);

            while (line != "")
            {
                message.ParseHeader(line);

                //Console.WriteLine(line);
                line = sr.ReadLine();
            }
            //}

            return message;
        }
    }
}
