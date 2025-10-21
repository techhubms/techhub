using System.Text.RegularExpressions;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace techhub.webapp.ApiService.Infrastructure.Parsers;

/// <summary>
/// Parses YAML frontmatter from markdown files.
/// Handles Jekyll-style frontmatter between --- delimiters.
/// </summary>
public class YamlFrontmatterParser
{
    private static readonly Regex FrontmatterRegex = new(
        @"^---\s*\n(.*?)\n---\s*\n",
        RegexOptions.Singleline | RegexOptions.Compiled
    );

    private readonly IDeserializer _deserializer;

    public YamlFrontmatterParser()
    {
        _deserializer = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .IgnoreUnmatchedProperties()
            .Build();
    }

    /// <summary>
    /// Parses frontmatter and content from markdown text.
    /// </summary>
    /// <param name="markdownText">Full markdown file content</param>
    /// <returns>Parsed frontmatter and remaining content</returns>
    public (Dictionary<string, object>? Frontmatter, string Content) Parse(string markdownText)
    {
        if (string.IsNullOrWhiteSpace(markdownText))
            return (null, string.Empty);

        var match = FrontmatterRegex.Match(markdownText);
        if (!match.Success)
            return (null, markdownText);

        try
        {
            var yamlText = match.Groups[1].Value;
            var frontmatter = _deserializer.Deserialize<Dictionary<string, object>>(yamlText);
            var content = markdownText[match.Length..];

            return (frontmatter, content);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to parse YAML frontmatter: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Extracts strongly-typed value from frontmatter dictionary.
    /// </summary>
    public T? GetValue<T>(Dictionary<string, object>? frontmatter, string key)
    {
        if (frontmatter == null || !frontmatter.TryGetValue(key, out var value))
            return default;

        try
        {
            if (value is T typedValue)
                return typedValue;

            // Handle type conversion
            return (T)Convert.ChangeType(value, typeof(T));
        }
        catch
        {
            return default;
        }
    }

    /// <summary>
    /// Gets string value from frontmatter.
    /// </summary>
    public string? GetString(Dictionary<string, object>? frontmatter, string key)
    {
        return GetValue<string>(frontmatter, key);
    }

    /// <summary>
    /// Gets string list value from frontmatter.
    /// </summary>
    public List<string> GetStringList(Dictionary<string, object>? frontmatter, string key)
    {
        if (frontmatter == null || !frontmatter.TryGetValue(key, out var value))
            return [];

        return value switch
        {
            List<object> list => list.Select(x => x?.ToString() ?? string.Empty).Where(s => !string.IsNullOrWhiteSpace(s)).ToList(),
            string[] array => array.Where(s => !string.IsNullOrWhiteSpace(s)).ToList(),
            string single => [single],
            _ => []
        };
    }

    /// <summary>
    /// Gets DateTimeOffset value from frontmatter.
    /// </summary>
    public DateTimeOffset? GetDate(Dictionary<string, object>? frontmatter, string key)
    {
        var value = GetString(frontmatter, key);
        if (string.IsNullOrWhiteSpace(value))
            return null;

        if (DateTimeOffset.TryParse(value, out var date))
            return date;

        return null;
    }
}
