using System;

namespace Genesis.Engine.Core.Logging
{
    public static class Logger
    {
        private static readonly object sync = new();

        public static void Info(string message)
        {
            Log("Info", message);
        }

        public static void Warn(string message)
        {
            Log("Warn", message);
        }

        public static void Error(string message)
        {
            Log("Error", message);
        }

        private static void Log(string level, string message)
        {
            lock (sync)
            {
                var ts = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                Console.WriteLine($"[{level}] {ts} {message}");
            }
        }
    }
}
