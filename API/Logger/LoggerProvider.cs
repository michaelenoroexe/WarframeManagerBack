namespace API.Logger
{
    internal sealed class LoggerProvider : ILoggerProvider
    {
        private readonly string _path;
        /// <summary>
        /// Configure provider with file path? to reate loggers.
        /// </summary>
        /// <param name="path">File path.</param>
        public LoggerProvider(string path) => _path = path;
        public ILogger CreateLogger(string categoryName) => new Logger(_path);
        public void Dispose() { }
    }
}
