using System;

namespace Genesis.Engine.Core.Logging
{
    public class LoggerService
    {
        public LoggerService()
        {
        }

        public void Info(string message) => Logger.Info(message);
        public void Warn(string message) => Logger.Warn(message);
        public void Error(string message) => Logger.Error(message);
    }
}
