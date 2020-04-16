using System;
using System.Collections.Generic;
using System.Text;

namespace ASPMajda.Server.Packet
{
    class Headers
    {
        public Dictionary<string, string> Data { get; set; }

        public Headers()
        {
            this.Data = new Dictionary<string, string>();
        }

        public void SetHeader(string name, string value)
        {
            if (!this.Data.ContainsKey(name))
                this.Data.Add(name, "");

            this.Data[name] = value;
        }

        public bool TryGetValue(string name, out string value)
        {
            value = "";
            if (!this.Data.ContainsKey(name)) return false;

            value = this.Data[name];
            return true;
        }
    }
}
