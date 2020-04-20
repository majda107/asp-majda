using ASPMajda.Server.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASPMajda.Server.Protection
{
    class BlacklistProtector : IProtector
    {
        public List<string> Hosts { get; private set; }
        public BlacklistProtector()
        {
            this.Hosts = new List<string>();
        }
        public bool Allowed(RequestMessage request)
        {
            string value = String.Empty;
            if (!request.Headers.TryGetValue("Host", out value)) return true;

            if (this.Hosts.Contains(value))
                return false;

            return true;
        }
    }
}
