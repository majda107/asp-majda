using System;
using System.Collections.Generic;
using System.Text;

namespace ASPMajda.Server.Logger
{
    interface ILogger
    {
        void Log(string message, Level level);

        void Warn(string message);
        void Error(string message);

        bool Warnings { get; set; }
        bool Errors { get; set; }
    }
}
