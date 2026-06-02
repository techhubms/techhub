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

    /// <summary>
    /// Masks an email address for safe logging, preserving only the first character of the local
    /// part and the full domain so logs remain useful without exposing PII.
    /// Examples: <c>user@example.com</c> → <c>u***@example.com</c>,
    ///           <c>a@b.com</c> → <c>a***@b.com</c>.
    /// </summary>
    public static string MaskEmail(this string? email)
    {
        if (string.IsNullOrEmpty(email))
        {
            return string.Empty;
        }

        var atIndex = email.IndexOf('@', StringComparison.Ordinal);
        if (atIndex <= 0)
        {
            // Not a recognisable email — sanitize only.
            return email.Sanitize();
        }

        return string.Concat(email.AsSpan(0, 1), "***", email.AsSpan(atIndex));
    }
}
