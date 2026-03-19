namespace TechHub.Core.Logging;

/// <summary>
/// Sanitizes user-controlled input to prevent log forging attacks.
/// Removes control characters (newlines, carriage returns) that could inject fake log entries.
/// Use as an extension method: <c>value.Sanitize()</c> on every user-controlled argument in log statements.
/// </summary>
public static class InputSanitizer
{
    /// <summary>
    /// Removes newline and carriage return characters from a value to prevent log forging.
    /// Can be called as <c>value.Sanitize()</c> (extension) or <c>InputSanitizer.Sanitize(value)</c>.
    /// </summary>
    public static string Sanitize(this string? value)
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
