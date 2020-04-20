using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ASPMajda.Server.Configuration
{
    interface IServerConfiguration: IConfiguration
    {
        int GetPort();
        IPAddress GetAddress();
    }
}
