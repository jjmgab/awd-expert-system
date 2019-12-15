using ExpertSystem.Enums;

namespace ExpertSystem.Helpers
{
    public delegate void LogEventHandler(object sender, LogEventArgs e);

    public class Logger
    {
        public event LogEventHandler Logged;

        /// <summary>
        /// Singleton instance.
        /// </summary>
        protected static Logger _instance;

        private Logger() { }

        /// <summary>
        /// Returns the singleton instance.
        /// </summary>
        public static Logger Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Logger();
                }
                return _instance;
            }
        }

        private static void Log(LogType type, string message)
        {
            LogEventArgs e = new LogEventArgs(type, message);
            Instance.OnLog(e);
        }

        protected void OnLog(LogEventArgs e)
        {
            Logged?.Invoke(this, e);
        }

        public static void Info(string message)
        {
            Log(LogType.Info, message);
        }

        public static void Warning(string message)
        {
            Log(LogType.Warning, message);
        }

        public static void Error(string message)
        {
            Log(LogType.Error, message);
        }

        public static void Fatal(string message)
        {
            Log(LogType.Fatal, message);
        }
    }
}
