using Microsoft.Extensions.Logging;

namespace TechHub.Infrastructure.Services.RoundupGeneration;

/// <summary>
/// Forwards each progress report to both the inner <see cref="IProgress{T}"/> (if any)
/// and the logger at Information level, so progress is visible in both the admin UI
/// and the structured log output.
/// </summary>
internal sealed class LoggingProgress
{
    private readonly ILogger _logger;
    private readonly IProgress<string>? _inner;

    internal LoggingProgress(ILogger logger, IProgress<string>? inner)
    {
        _logger = logger;
        _inner = inner;
    }

    internal void Report(string message)
    {
        _logger.LogInformation("{ProgressMessage}", message);
        _inner?.Report(message);
    }
}
