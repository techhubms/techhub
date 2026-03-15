namespace TechHub.Core.Models;

/// <summary>
/// Represents a collection with its metadata.
/// </summary>
public record Collection
{
    public string Name { get; }
    public string Title { get; }
    public string Url { get; }
    public string Description { get; }
    public string DisplayName { get; }
    public bool IsCustom { get; }

    /// <summary>
    /// Display order for custom pages (lower values appear first).
    /// Only used when IsCustom=true. Defaults to 0.
    /// </summary>
    public int Order { get; }

    public Collection(
        string name,
        string title,
        string url,
        string description,
        string displayName,
        bool isCustom = false,
        int order = 0)
    {
        // Validate all required properties
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Collection name cannot be empty", nameof(name));
        }

        if (!name.All(c => char.IsLower(c) || c == '-'))
        {
            throw new ArgumentException("Collection name must be lowercase with hyphens only", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Collection title cannot be empty", nameof(title));
        }

        if (string.IsNullOrWhiteSpace(url))
        {
            throw new ArgumentException("Collection URL cannot be empty", nameof(url));
        }

        if (!url.StartsWith('/'))
        {
            throw new ArgumentException("Collection URL must start with '/'", nameof(url));
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("Collection description cannot be empty", nameof(description));
        }

        if (string.IsNullOrWhiteSpace(displayName))
        {
            throw new ArgumentException("Collection display name cannot be empty", nameof(displayName));
        }

        Name = name;
        Title = title;
        Url = url;
        Description = description;
        DisplayName = displayName;
        IsCustom = isCustom;
        Order = order;
    }

    /// <summary>
    /// Generate a tag from a collection name by replacing dashes with spaces
    /// and uppercasing the first letter of each word.
    /// Examples: "blogs" -> "Blogs", "vscode-updates" -> "Vscode Updates"
    /// </summary>
    public static string GetTagFromName(string collectionName)
    {
        if (string.IsNullOrWhiteSpace(collectionName))
        {
            return string.Empty;
        }

        // Replace dashes with spaces and split into words
        var words = collectionName.Replace('-', ' ').Split(' ', StringSplitOptions.RemoveEmptyEntries);

        // Uppercase first letter of each word
        for (int i = 0; i < words.Length; i++)
        {
            if (words[i].Length > 0)
            {
                words[i] = char.ToUpperInvariant(words[i][0]) + words[i][1..];
            }
        }

        return string.Join(' ', words);
    }
}
