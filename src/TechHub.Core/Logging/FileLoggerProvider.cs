using Microsoft.Extensions.Logging;

namespace TechHub.Core.Logging;

/// <summary>
/// Simple file logger provider for application logs with Europe/Brussels timezone.
/// Respects log level configuration passed as a dictionary.
/// </summary>
public sealed class FileLoggerProvider : ILoggerProvider
{
    private readonly StreamWriter _writer;
    private readonly object _lock = new();
    private readonly Dictionary<string, LogLevel> _logLevels;
    private readonly LogLevel _defaultLogLevel;
    private static readonly TimeZoneInfo _brusselsTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Brussels");

    /// <summary>
    /// Creates a file logger provider with optional log level configuration.
    /// </summary>
    /// <param name="filePath">Path to the log file.</param>
    /// <param name="logLevels">Dictionary of category prefixes to minimum log levels. Use "Default" key for default level.</param>
    public FileLoggerProvider(string filePath, IDictionary<string, LogLevel>? logLevels = null)
    {
        var directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        _writer = new StreamWriter(filePath, append: true) { AutoFlush = true };

        // Copy log levels from provided dictionary
        _logLevels = new Dictionary<string, LogLevel>(StringComparer.OrdinalIgnoreCase);
        _defaultLogLevel = LogLevel.Information;

        if (logLevels != null)
        {
            foreach (var (key, level) in logLevels)
            {
                if (key.Equals("Default", StringComparison.OrdinalIgnoreCase))
                {
                    _defaultLogLevel = level;
                }
                else
                {
                    _logLevels[key] = level;
                }
            }
        }
    }

    public ILogger CreateLogger(string categoryName)
    {
        ArgumentNullException.ThrowIfNull(categoryName);
        return new FileLogger(categoryName, _writer, _lock, GetMinLogLevel(categoryName));
    }

    private LogLevel GetMinLogLevel(string categoryName)
    {
        // Find the most specific matching prefix
        var bestMatch = (Prefix: "", Level: _defaultLogLevel);

        foreach (var (prefix, level) in _logLevels)
        {
            if (categoryName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase) &&
                prefix.Length > bestMatch.Prefix.Length)
            {
                bestMatch = (prefix, level);
            }
        }

        return bestMatch.Level;
    }

    public void Dispose()
    {
        _writer.Dispose();
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Simple file logger for application with local timezone timestamps
    /// </summary>
    private sealed class FileLogger : ILogger
    {
        private readonly string _categoryName;
        private readonly StreamWriter _writer;
        private readonly object _lock;
        private readonly LogLevel _minLogLevel;

        public FileLogger(string categoryName, StreamWriter writer, object lockObj, LogLevel minLogLevel)
        {
            ArgumentNullException.ThrowIfNull(categoryName);
            ArgumentNullException.ThrowIfNull(writer);
            ArgumentNullException.ThrowIfNull(lockObj);

            _categoryName = categoryName;
            _writer = writer;
            _lock = lockObj;
            _minLogLevel = minLogLevel;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => logLevel >= _minLogLevel;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            lock (_lock)
            {
                // Convert UTC to Europe/Brussels timezone (handles CET/CEST automatically)
                var localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _brusselsTimeZone);
                _writer.WriteLine($"[{localTime:yyyy-MM-dd HH:mm:ss.fff}] [{logLevel}] {_categoryName}: {formatter(state, exception)}");
                if (exception != null)
                {
                    _writer.WriteLine(exception.ToString());
                }
            }
        }
    }
}
