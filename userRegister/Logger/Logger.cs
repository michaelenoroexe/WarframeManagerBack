namespace API.Logger
{
    public sealed class Logger:ILogger, IDisposable
    {
        private readonly string _file;
        static object _lock = new object();

        public Logger(string file)
        {  
            string? _dir = Path.GetDirectoryName(file);
            if (_dir == null || _dir == "") throw new Exception("Invalid Directory");
            Directory.CreateDirectory(_dir);
            string? _fileName = Path.GetFileName(file);
            if (_fileName == null || _fileName == "") throw new Exception("Invalid FileName");
            if (!Path.HasExtension(file)) file += ".txt";
            if (!File.Exists(file)) File.Create(file);
            _file = file;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return this;
        }

        public void Dispose() { }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }
        // Define logging logic
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            SaveLogToFile<TState>(eventId, state, exception);
        }
        // Saving log to file
        private void SaveLogToFile<TState>(EventId evId, TState state, Exception? exception)
        {
            string time = DateTime.Now.ToString() + ": ";
            lock (_lock)
            {
                File.AppendAllText(_file, "Date: " + time + " State: " + state.ToString() + FormatExc(evId, exception) + Environment.NewLine);
            }
        }
        // Formating accepted info
        private string FormatExc( EventId evId, Exception? exception)
        {
            if (exception == null) return "";
            string splitStr = $"{Environment.NewLine}--- End of stack trace from previous location ---";
            return " Event: " + evId.Name + " Exception Message: " + exception.Message + Environment.NewLine + "Stack Trace" + exception.StackTrace.Split(splitStr).FirstOrDefault();
        }
    }
}
