using ASPMajda.Server.Content;
using ASPMajda.Server.Packet;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASPMajda.Server.Messages
{
    class RequestMessage
    {
        public Method Method { get; private set; } = Method.INVALID;
        public string Path { get; private set; }
        public string Version { get; private set; }
        public Headers Headers { get; private set; }

        public bool HasBody
        {
            get => Method == Method.POST || Method == Method.PUT || Method == Method.CONNECT || Method == Method.PATCH;
        }
        public IContent Body { get; set; }

        public RequestMessage()
        {
            this.Headers = new Headers();
        }

        public void ParsePath(string line)
        {
            if (line == null) return;

            var split = line.Split(' ');
            if (split.Length < 3) return;

            Method method;
            if (!Enum.TryParse<Method>(split[0], out method))
                method = Method.INVALID;

            this.Method = method;
            this.Path = split[1];
            this.Version = split[2];
        }
        public void ParseHeader(string line)
        {
            if (line == null) return;

            var split = line.Split(": ");
            if (split.Length < 2) return;

            this.Headers.SetHeader(split[0], split[1]);
        }


        public bool TryGetContentLength(out int contentLength)
        {
            contentLength = 0;

            string val;
            if (!this.Headers.TryGetValue("Content-Length", out val)) return false;
            return int.TryParse(val, out contentLength);
        }

        public bool TryGetContentType(out string contentType)
        {
            if (this.Headers.TryGetValue("Content-Type", out contentType)) return true;

            contentType = "application/octet-stream";
            return false;
        }
    }
}
