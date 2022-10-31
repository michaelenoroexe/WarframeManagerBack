namespace API.Logger
{
    internal sealed class LoggerProvider : ILoggerProvider
    {
        private string _path;
        public LoggerProvider(string path) => _path = path;
        public ILogger CreateLogger(string categoryName) => new Logger(_path);
        public void Dispose() { }
    }
}
