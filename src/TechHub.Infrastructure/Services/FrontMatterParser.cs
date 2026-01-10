using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace TechHub.Infrastructure.Services;

/// <summary>
/// Parses YAML frontmatter from markdown files
/// Handles the format: ---\nkey: value\n---\ncontent
/// </summary>
public class FrontMatterParser
{
    private readonly IDeserializer _deserializer;

    public FrontMatterParser()
    {
        _deserializer = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .IgnoreUnmatchedProperties()
            .Build();
    }

    /// <summary>
    /// Parse markdown file into frontmatter metadata and content body
    /// </summary>
    /// <param name="markdownContent">Full markdown file content with frontmatter</param>
    /// <returns>Tuple of (frontmatter dictionary, content body)</returns>
    public (Dictionary<string, object> FrontMatter, string Content) Parse(string markdownContent)
    {
        if (string.IsNullOrWhiteSpace(markdownContent))
        {
            return (new Dictionary<string, object>(), string.Empty);
        }

        // Frontmatter must start with ---
        if (!markdownContent.TrimStart().StartsWith("---", StringComparison.Ordinal))
        {
            return (new Dictionary<string, object>(), markdownContent);
        }

        var lines = markdownContent.Split('\n');
        var yamlLines = new List<string>();
        var contentLines = new List<string>();
        var inFrontMatter = false;
        var frontMatterClosed = false;

        foreach (var line in lines)
        {
            // First --- starts frontmatter
            if (!inFrontMatter && line.TrimStart().StartsWith("---", StringComparison.Ordinal))
            {
                inFrontMatter = true;
                continue;
            }

            // Second --- ends frontmatter
            if (inFrontMatter && !frontMatterClosed && line.TrimStart().StartsWith("---", StringComparison.Ordinal))
            {
                frontMatterClosed = true;
                inFrontMatter = false;
                continue;
            }

            // Collect YAML lines
            if (inFrontMatter)
            {
                yamlLines.Add(line);
            }
            // Collect content lines (after frontmatter closed)
            else if (frontMatterClosed)
            {
                contentLines.Add(line);
            }
        }

        // Parse YAML frontmatter
        var frontMatter = new Dictionary<string, object>();
        if (yamlLines.Count > 0)
        {
            var yaml = string.Join('\n', yamlLines);
            try
            {
                var parsed = _deserializer.Deserialize<Dictionary<string, object>>(yaml);
                frontMatter = parsed ?? [];
            }
            catch (YamlDotNet.Core.YamlException)
            {
                // If YAML parsing fails, return empty frontmatter and original content
                return (new Dictionary<string, object>(), markdownContent);
            }
        }

        var content = string.Join('\n', contentLines).TrimStart();
        return (frontMatter, content);
    }

    /// <summary>
    /// Get a frontmatter value by key, or return default if not found
    /// </summary>
    public T GetValue<T>(Dictionary<string, object> frontMatter, string key, T defaultValue = default!)
    {
        ArgumentNullException.ThrowIfNull(frontMatter);
        if (!frontMatter.TryGetValue(key, out var value))
        {
            return defaultValue;
        }

        try
        {
            return (T)Convert.ChangeType(value, typeof(T), System.Globalization.CultureInfo.InvariantCulture);
        }
        catch
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// Get a list value from frontmatter (e.g., tags, categories)
    /// </summary>
    public List<string> GetListValue(Dictionary<string, object> frontMatter, string key)
    {
        ArgumentNullException.ThrowIfNull(frontMatter);
        if (!frontMatter.TryGetValue(key, out var value))
        {
            return [];
        }

        // Handle various formats: string, List<object>, object[]
        return value switch
        {
            string str => [str],
            IEnumerable<object> list => [.. list.Select(x => x.ToString() ?? string.Empty)],
            _ => []
        };
    }
}
