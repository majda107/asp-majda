using ASPMajda.Server.Connection;
using ASPMajda.Server.Controller;
using ASPMajda.Server.Logger;
using ASPMajda.Server.Messages;
using ASPMajda.Server.Content;
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

        public IPAddress Address { get; private set; }
        public int Port { get; private set; }

        private TcpListener listener;


        public ServiceManager ServiceManager { get; private set; }


        public HttpServer(IPAddress address, int port)
        {
            this.Address = address;
            this.Port = port;

            this.listener = new TcpListener(address, port);

            this.Pool = new Pool();

            this.ServiceManager = new ServiceManager();
        }

        

        public void Listen()
        {
            this.listener.Start();
            while (true)
            {
                var client = new Client(listener.AcceptTcpClient());
                this.ServiceManager.HandleLog("Received connection...", Level.Info);

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
                var request = this.HandleRequest(sr);
                if (request.HasBody)
                    this.HandleBody(sr, ref request);

                ResponseMessage response = ResponseMessage.Error;
                this.ServiceManager.HandleControllers(request, out response);

                this.HandleResponse(sw, response);
                client.Stream.Close();
            }

            this.Pool.Remove(client);
        }

        private void HandleBody(StreamReader sr, ref RequestMessage message)
        {
            int len;
            string type;
            if (!message.TryGetContentLength(out len)) return;
            message.TryGetContentType(out type);

            this.ServiceManager.HandleLog($"Found {type} body content", Level.Info);

            if (type.StartsWith("text") || type == "application/json")
            {
                char[] buffer = new char[len];
                sr.Read(buffer, 0, len);
                message.Body = new StringContent(new String(buffer));
                this.ServiceManager.HandleLog($"Extracted text content: {message.Body}", Level.Detailed);
            }            
        }

        private void HandleResponse(StreamWriter sw, ResponseMessage response)
        {
            if (response == null) response = ResponseMessage.Error;

            response.Headers.SetHeader("Server", "ASPMajda/1.0 (CSharp)");
            response.Headers.SetHeader("Date", DateTime.UtcNow.ToString());

            this.ServiceManager.HandleLog($"Sending response... {response.StatusCode} with {response.Headers.Data.Count} headers", Level.Info);

            sw.WriteLine($"HTTP/1.1 ${response.StatusCode} OK");
            foreach (var header in response.Headers.Data)
                sw.WriteLine($"{header.Key}: {header.Value}");

            if (response.Content != null)
            {
                this.ServiceManager.HandleLog($"Sending content", Level.Detailed);

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
            
            var line = sr.ReadLine();
            message.ParsePath(line);

            while (line != "")
            {
                message.ParseHeader(line);
                line = sr.ReadLine();
            }

            this.ServiceManager.HandleLog($"Request... method: {message.Method}, path: {message.Path}", Level.Detailed);
            return message;
        }
    }
}
