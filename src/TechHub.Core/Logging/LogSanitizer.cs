namespace TechHub.Core.Logging;

/// <summary>
/// Sanitizes values for safe use in log messages to prevent log forging attacks.
/// Removes control characters (newlines, carriage returns) that could inject fake log entries.
/// </summary>
public static class LogSanitizer
{
    /// <summary>
    /// Removes newline and carriage return characters from a value to prevent log forging.
    /// </summary>
    public static string Sanitize(string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        return value
            .Replace("\r", string.Empty, StringComparison.Ordinal)
            .Replace("\n", string.Empty, StringComparison.Ordinal);
    }
}
