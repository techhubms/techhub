using Microsoft.Extensions.Logging;

namespace TechHub.Core.Logging;

/// <summary>
/// Extension methods for <see cref="ILogger"/> that automatically sanitize all string arguments
/// to prevent log forging attacks. Use these instead of the standard Log* methods whenever
/// arguments may contain user-controlled input.
/// </summary>
public static class LoggerSanitizedExtensions
{
#pragma warning disable CA2254 // Message template is intentionally forwarded in this sanitizing wrapper
    /// <summary>Logs a debug message, sanitizing all string arguments.</summary>
    public static void LogDebugSanitized(this ILogger logger, string message, params object?[] args)
        => logger.LogDebug(message, SanitizeArgs(args));

    /// <summary>Logs an information message, sanitizing all string arguments.</summary>
    public static void LogInformationSanitized(this ILogger logger, string message, params object?[] args)
        => logger.LogInformation(message, SanitizeArgs(args));

    /// <summary>Logs a warning message, sanitizing all string arguments.</summary>
    public static void LogWarningSanitized(this ILogger logger, string message, params object?[] args)
        => logger.LogWarning(message, SanitizeArgs(args));

    /// <summary>Logs an error message with an exception, sanitizing all string arguments.</summary>
    public static void LogErrorSanitized(this ILogger logger, Exception exception, string message, params object?[] args)
        => logger.LogError(exception, message, SanitizeArgs(args));

    /// <summary>Logs an error message, sanitizing all string arguments.</summary>
    public static void LogErrorSanitized(this ILogger logger, string message, params object?[] args)
        => logger.LogError(message, SanitizeArgs(args));
#pragma warning restore CA2254

    private static object?[] SanitizeArgs(object?[] args) =>
        Array.ConvertAll(args, a => a is string s ? (object?)LogSanitizer.Sanitize(s) : a);
}
