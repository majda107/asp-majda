using System;
using System.Collections.Generic;
using System.Text;

namespace ASPMajda.Server.Logger
{
    interface ILogger
    {
        void Log(string message, Level level);
    }
}
