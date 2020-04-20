using System;
using System.Collections.Generic;
using System.Text;

namespace ASPMajda.Server.Packet
{
    class QueryParams: Dictionary<string, string>
    {
        public void SetQueryParam(string name, string value)
        {
            if (!this.ContainsKey(name))
                this.Add(name, value);

            this[name] = value;
        }

        public void Parse(string urlParams)
        {
            var split = urlParams.Split('&');
            foreach(var pair in split)
            {
                var kvp = pair.Split('=');
                if (kvp.Length < 2) continue;

                this.SetQueryParam(kvp[0], kvp[1]);
            }
        }
    }
}
