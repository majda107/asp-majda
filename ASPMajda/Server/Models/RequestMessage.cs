using System;
using System.Collections.Generic;
using System.Text;

namespace ASPMajda.Server.Models
{
    class RequestMessage
    {
        public Method Method { get; private set; }
        public string Path { get; private set; }
        public string Version { get; private set; }

        public Dictionary<string, string> Headers { get; private set; }

        public RequestMessage()
        {
            this.Headers = new Dictionary<string, string>();
        }

        public void ParsePath(string line)
        {
            if (line == null) return;

            var split = line.Split(' ');
            this.Method = Enum.Parse<Method>(split[0]);
            this.Path = split[1];
            this.Version = split[2];
        }
        public void ParseHeader(string line)
        {
            //var split = line.Split(' ');
            //if(split.Length == 3)
            //{

            //}
            var split = line.Split(": ");
            if (split.Length < 2) return;

            this.Headers.Add(split[0], split[1]);
        }
    }
}
