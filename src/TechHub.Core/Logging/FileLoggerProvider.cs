using Microsoft.Extensions.Logging;

namespace TechHub.Core.Logging;

/// <summary>
/// Simple file logger provider for application logs with Europe/Brussels timezone
/// </summary>
public sealed class FileLoggerProvider : ILoggerProvider
{
    private readonly string _filePath;
    private readonly StreamWriter _writer;
    private readonly object _lock = new();
    private static readonly TimeZoneInfo BrusselsTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Brussels");

    public FileLoggerProvider(string filePath)
    {
        _filePath = filePath;
        var directory = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        _writer = new StreamWriter(_filePath, append: true) { AutoFlush = true };
    }

    public ILogger CreateLogger(string categoryName) => new FileLogger(categoryName, _writer, _lock);

    public void Dispose()
    {
        _writer.Dispose();
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Simple file logger for application with local timezone timestamps
    /// </summary>
    private sealed class FileLogger(string categoryName, StreamWriter writer, object lockObj) : ILogger
    {
        private readonly string _categoryName = categoryName;
        private readonly StreamWriter _writer = writer;
        private readonly object _lock = lockObj;

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => logLevel >= LogLevel.Information;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            lock (_lock)
            {
                // Convert UTC to Europe/Brussels timezone (handles CET/CEST automatically)
                var localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, BrusselsTimeZone);
                _writer.WriteLine($"[{localTime:yyyy-MM-dd HH:mm:ss.fff}] [{logLevel}] {_categoryName}: {formatter(state, exception)}");
                if (exception != null)
                {
                    _writer.WriteLine(exception.ToString());
                }
            }
        }
    }
}
