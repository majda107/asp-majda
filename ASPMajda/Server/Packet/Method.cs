using System;
using System.Collections.Generic;
using System.Text;

namespace ASPMajda.Server.Packet
{
    enum Method
    {
        GET,
        HEAD,
        POST,
        PUT,
        DELETE,
        CONNECT,
        OPTIONS,
        TRACE,
        PATCH,
        INVALID
    }
}
