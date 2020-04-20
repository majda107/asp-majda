using System;
using System.Collections.Generic;
using System.Text;

namespace ASPMajda.Server.Logger
{
    class ConsoleLogger: ILogger
    {

        public Level Level { get; private set; }
        public bool Warnings { get; set; }
        public bool Errors { get; set; }

        public ConsoleLogger(Level level = Level.Info)
        {
            this.Level = level;

            this.Warnings = true;
            this.Errors = true;
        }

        private void PrintColor(string message, ConsoleColor color)
        {
            var tmp = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(message);
            Console.ForegroundColor = tmp;
        }

        public void Log(string message, Level level)
        {
            if (this.Level > level) return;

            Console.WriteLine($"[{level.ToString()}] {message}");
        }

        public void Warn(string message)
        {
            if (!this.Warnings) return;
            this.PrintColor("[Warning] ", ConsoleColor.DarkYellow);
            Console.WriteLine(message);
        }

        public void Error(string message)
        {
            if (!this.Errors) return;
            this.PrintColor("[Error] ", ConsoleColor.DarkRed);
            Console.WriteLine(message);
        }
    }
}
