using ASPMajda.Server.Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASPMajda.Server.Configuration
{
    interface IConfiguration
    {
        void ModifyServer(HttpServer server);
    }
}
