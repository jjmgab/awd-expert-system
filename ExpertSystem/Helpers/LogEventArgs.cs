using ExpertSystem.Enums;
using System;

namespace ExpertSystem.Helpers
{
    public class LogEventArgs : EventArgs
    {
        private LogType _type;
        public LogType Type => _type;

        private string _message;
        public string Message => _message;

        public LogEventArgs(LogType type, string message)
        {
            _type = type;
            _message = message;
        }
    }
}
