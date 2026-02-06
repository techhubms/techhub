using System.Collections.Concurrent;
using System.Text;
using Microsoft.Extensions.Logging;

namespace TechHub.Core.Logging;

/// <summary>
/// High-performance file logger provider with background write queue for concurrent logging.
/// Uses Europe/Brussels timezone and respects log level configuration.
/// Thread-safe across multiple processes via per-write file locking.
/// </summary>
public sealed class FileLoggerProvider : ILoggerProvider
{
    private readonly string _filePath;
    private readonly Dictionary<string, LogLevel> _logLevels;
    private readonly LogLevel _defaultLogLevel;
    private static readonly TimeZoneInfo _brusselsTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Brussels");

    // High-performance concurrent write queue
    private readonly BlockingCollection<string> _writeQueue;
    private readonly Thread _writerThread;
    private readonly CancellationTokenSource _cts;

    /// <summary>
    /// Creates a file logger provider with optional log level configuration.
    /// Uses a background thread for non-blocking, thread-safe writes.
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

        _filePath = filePath;

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

        // Initialize background writer thread for non-blocking concurrent writes
        _writeQueue = new BlockingCollection<string>(boundedCapacity: 10000);
        _cts = new CancellationTokenSource();
        _writerThread = new Thread(ProcessWriteQueue)
        {
            IsBackground = true,
            Name = "FileLogger-Writer"
        };
        _writerThread.Start();
    }

    private void ProcessWriteQueue()
    {
        try
        {
            while (!_cts.Token.IsCancellationRequested)
            {
                // GetConsumingEnumerable blocks until items available
                foreach (var logEntry in _writeQueue.GetConsumingEnumerable(_cts.Token))
                {
                    // Retry with exclusive lock (FileShare.None) - handle parallel test processes
                    const int maxRetries = 100; // 100ms max wait (100 retries × 1ms)
                    for (int i = 0; i < maxRetries; i++)
                    {
                        try
                        {
                            using (var fileStream = new FileStream(_filePath, FileMode.Append, FileAccess.Write, FileShare.None))
                            using (var writer = new StreamWriter(fileStream, Encoding.UTF8))
                            {
                                writer.WriteLine(logEntry);
                            }

                            break; // Success - exit retry loop
                        }
                        catch (IOException) when (i < maxRetries - 1)
                        {
                            // File locked by another process - wait 1ms and retry
                            Thread.Sleep(1);
                        }
#pragma warning disable CA1031 // Logging must not throw - swallow all exceptions in background write thread
                        catch
                        {
                            // Other errors (permissions, disk full, etc.) - give up
                            break;
                        }
#pragma warning restore CA1031
                    }
                }
            }
        }
        catch (OperationCanceledException)
        {
            // Expected during shutdown
        }
    }

    public ILogger CreateLogger(string categoryName)
    {
        ArgumentNullException.ThrowIfNull(categoryName);
        return new FileLogger(categoryName, _writeQueue, GetMinLogLevel(categoryName));
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
        // Signal shutdown
        _cts.Cancel();
        _writeQueue.CompleteAdding();

        // Wait for writer thread to finish (max 5 seconds)
        _writerThread.Join(TimeSpan.FromSeconds(5));

        _writeQueue.Dispose();
        _cts.Dispose();

        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Lock-free file logger that queues writes to a background thread
    /// </summary>
    private sealed class FileLogger : ILogger
    {
        private readonly string _categoryName;
        private readonly BlockingCollection<string> _writeQueue;
        private readonly LogLevel _minLogLevel;

        public FileLogger(string categoryName, BlockingCollection<string> writeQueue, LogLevel minLogLevel)
        {
            ArgumentNullException.ThrowIfNull(categoryName);
            ArgumentNullException.ThrowIfNull(writeQueue);

            _categoryName = categoryName;
            _writeQueue = writeQueue;
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

            // Convert UTC to Europe/Brussels timezone (handles CET/CEST automatically)
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _brusselsTimeZone);
            var logEntry = new StringBuilder(256);
            logEntry.Append('[')
                    .Append(localTime.ToString("yyyy-MM-dd HH:mm:ss.fff"))
                    .Append("] [")
                    .Append(logLevel)
                    .Append("] ")
                    .Append(_categoryName)
                    .Append(": ")
                    .Append(formatter(state, exception));

            // Non-blocking write to queue - if queue is full, TryAdd will fail gracefully
            // This prevents logging from blocking application threads
            if (!_writeQueue.TryAdd(logEntry.ToString(), millisecondsTimeout: 100))
            {
                // Queue is full - log entry is dropped to prevent blocking
                // This only happens under extreme load (10000+ queued messages)
            }

            // If exception exists, queue it as a separate entry
            if (exception != null)
            {
                _writeQueue.TryAdd(exception.ToString(), millisecondsTimeout: 100);
            }
        }
    }
}
