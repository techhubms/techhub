namespace techhub.webapp.ApiService.Domain.Models;

/// <summary>
/// Represents a content tag with normalization for filtering.
/// </summary>
public class Tag
{
    /// <summary>
    /// Unique identifier.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Display format (e.g., "Visual Studio Code").
    /// </summary>
    public required string Display { get; set; }

    /// <summary>
    /// Normalized format for filtering (e.g., "visual studio code").
    /// </summary>
    public string Normalized => NormalizeTag(Display);

    /// <summary>
    /// Number of times this tag is used.
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    /// Date when this tag first appeared.
    /// </summary>
    public DateTimeOffset FirstSeen { get; set; } = DateTimeOffset.UtcNow;

    /// <summary>
    /// Tags that subset-match this tag (for filtering).
    /// </summary>
    public List<string> RelatedTags { get; set; } = [];

    /// <summary>
    /// Normalizes a tag display string to filtering format.
    /// </summary>
    /// <param name="display">Display format tag (e.g., "Visual Studio Code")</param>
    /// <returns>Normalized format (e.g., "visual studio code")</returns>
    public static string NormalizeTag(string display)
    {
        if (string.IsNullOrWhiteSpace(display))
            return string.Empty;

        var normalized = display.ToLowerInvariant();

        // Handle special cases
        normalized = normalized switch
        {
            var s when s.Contains("c#") => s.Replace("c#", "csharp"),
            var s when s.Contains("f#") => s.Replace("f#", "fsharp"),
            var s when s.Contains("+") => s.Replace("+", "plus"),
            var s when s.Contains("++") => s.Replace("++", "plusplus"),
            _ => normalized
        };

        // Replace special characters (except hyphens) with spaces
        normalized = System.Text.RegularExpressions.Regex.Replace(normalized, @"[^a-z0-9\s\-]", " ");

        // Collapse multiple spaces to single space
        normalized = System.Text.RegularExpressions.Regex.Replace(normalized, @"\s+", " ");

        // Trim whitespace
        normalized = normalized.Trim();

        return normalized;
    }

    /// <summary>
    /// Checks if this tag subset-matches another tag.
    /// </summary>
    /// <param name="otherTag">Tag to compare against</param>
    /// <returns>True if this tag is contained as complete words in the other tag</returns>
    public bool IsSubsetOf(string otherTag)
    {
        var normalizedOther = NormalizeTag(otherTag);
        var normalizedThis = Normalized;

        if (normalizedThis == normalizedOther)
            return true;

        // Check word boundary matching
        var pattern = $@"\b{System.Text.RegularExpressions.Regex.Escape(normalizedThis)}\b";
        return System.Text.RegularExpressions.Regex.IsMatch(normalizedOther, pattern);
    }

    /// <summary>
    /// Validates the tag according to business rules.
    /// </summary>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Display))
            throw new ArgumentException("Display cannot be empty", nameof(Display));

        if (Count < 0)
            throw new ArgumentException("Count cannot be negative", nameof(Count));
    }
}
