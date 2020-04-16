using ASPMajda.Server.Packet;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASPMajda.Models.Attributes
{
    class FromMethod: System.Attribute
    {
        public Method Method { get; private set; }
        public FromMethod(Method method)
        {
            this.Method = method;
        }
    }
}
