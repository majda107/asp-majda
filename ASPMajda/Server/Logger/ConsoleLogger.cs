using System;
using System.Collections.Generic;
using System.Text;

namespace ASPMajda.Server.Logger
{
    class ConsoleLogger: ILogger
    {

        public Level Level { get; private set; }
        public ConsoleLogger(Level level = Level.Info)
        {
            this.Level = level;
        }

        public void Log(string message, Level level)
        {
            if (this.Level > level) return;

            Console.WriteLine($"[{level.ToString()}] {message}");
        }
    }
}
