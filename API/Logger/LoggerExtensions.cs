namespace API.Logger
{
    public static class LoggerExtensions
    {
        public static ILoggingBuilder AddLog(this ILoggingBuilder builder, string filePath)
        {
            builder.AddProvider(new LoggerProvider(filePath));
            return builder;
        }
    }
}
