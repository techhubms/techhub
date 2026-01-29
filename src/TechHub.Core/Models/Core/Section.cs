namespace TechHub.Core.Models;

/// <summary>
/// Represents a section with its collections and metadata.
/// </summary>
public record Section
{
    public string Name { get; }
    public string Title { get; }
    public string Description { get; }
    public string Url { get; }
    public IReadOnlyList<Collection> Collections { get; }

    public Section(
        string name,
        string title,
        string description,
        string url,
        IReadOnlyList<Collection> collections)
    {
        // Validate all required properties
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Section name cannot be empty", nameof(name));
        }

        if (!name.All(c => char.IsLower(c) || c == '-'))
        {
            throw new ArgumentException("Section name must be lowercase with hyphens only", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Section title cannot be empty", nameof(title));
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("Section description cannot be empty", nameof(description));
        }

        if (string.IsNullOrWhiteSpace(url))
        {
            throw new ArgumentException("Section URL cannot be empty", nameof(url));
        }

        if (!url.StartsWith('/'))
        {
            throw new ArgumentException("Section URL must start with '/'", nameof(url));
        }

        ArgumentNullException.ThrowIfNull(collections);
        if (collections.Count == 0)
        {
            throw new ArgumentException("Section must have at least one collection", nameof(collections));
        }

        Name = name;
        Title = title;
        Description = description;
        Url = url;
        Collections = collections;
    }
}
