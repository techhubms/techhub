using System.Text.RegularExpressions;

namespace TechHub.Core.Validation;

/// <summary>
/// Validates route path parameters (section names, collection names, slugs)
/// to reject obviously malicious or malformed input early — before it hits
/// the repository layer or gets interpolated into URLs.
/// </summary>
public static partial class RouteParameterValidator
{
    /// <summary>
    /// Maximum length for any route path segment.
    /// </summary>
    private const int MaxSegmentLength = 200;

    /// <summary>
    /// Validates a section or collection name.
    /// Must be lowercase letters and hyphens only (matching Section/Collection constructors).
    /// </summary>
    public static bool IsValidNameSegment(string? value)
    {
        if (string.IsNullOrEmpty(value) || value.Length > MaxSegmentLength)
        {
            return false;
        }

        return NameSegmentRegex().IsMatch(value);
    }

    /// <summary>
    /// Validates a content slug.
    /// Slugs are derived from markdown filenames: lowercase letters, digits, and hyphens.
    /// </summary>
    public static bool IsValidSlug(string? value)
    {
        if (string.IsNullOrEmpty(value) || value.Length > MaxSegmentLength)
        {
            return false;
        }

        return SlugRegex().IsMatch(value);
    }

    /// <summary>
    /// Lowercase letters and hyphens only, must start with a letter.
    /// Matches the Section and Collection constructor validation.
    /// </summary>
    [GeneratedRegex(@"^[a-z][a-z-]*$", RegexOptions.Compiled)]
    private static partial Regex NameSegmentRegex();

    /// <summary>
    /// Lowercase letters, digits, and hyphens. Must start with a letter or digit.
    /// No dots (blocks .php probes), no slashes, no special characters.
    /// </summary>
    [GeneratedRegex(@"^[a-z0-9][a-z0-9-]*$", RegexOptions.Compiled)]
    private static partial Regex SlugRegex();
}
