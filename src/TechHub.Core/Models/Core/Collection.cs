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

    public Collection(
        string name,
        string title,
        string url,
        string description,
        string displayName,
        bool isCustom = false)
    {
        // Validate all required properties
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Collection name cannot be empty", nameof(name));

        if (!name.All(c => char.IsLower(c) || c == '-'))
            throw new ArgumentException("Collection name must be lowercase with hyphens only", nameof(name));

        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Collection title cannot be empty", nameof(title));

        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("Collection URL cannot be empty", nameof(url));

        if (!url.StartsWith('/'))
            throw new ArgumentException("Collection URL must start with '/'", nameof(url));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Collection description cannot be empty", nameof(description));

        if (string.IsNullOrWhiteSpace(displayName))
            throw new ArgumentException("Collection display name cannot be empty", nameof(displayName));

        Name = name;
        Title = title;
        Url = url;
        Description = description;
        DisplayName = displayName;
        IsCustom = isCustom;
    }
}
