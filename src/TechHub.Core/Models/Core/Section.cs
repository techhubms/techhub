namespace TechHub.Core.Models;

/// <summary>
/// DTO for section information returned from API
/// </summary>
public record Section
{
    public required string Name { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Url { get; init; }
    public required IReadOnlyList<Collection> Collections { get; init; }

    /// <summary>
    /// Validates that all required properties are correctly formatted
    /// </summary>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            throw new ArgumentException("Section name cannot be empty", nameof(Name));
        }

        if (!Name.All(c => char.IsLower(c) || c == '-'))
        {
            throw new ArgumentException("Section name must be lowercase with hyphens only", nameof(Name));
        }

        if (string.IsNullOrWhiteSpace(Title))
        {
            throw new ArgumentException("Section title cannot be empty", nameof(Title));
        }

        if (string.IsNullOrWhiteSpace(Url))
        {
            throw new ArgumentException("Section URL cannot be empty", nameof(Url));
        }

        if (!Url.StartsWith('/'))
        {
            throw new ArgumentException("Section URL must start with '/'", nameof(Url));
        }

        if (Collections.Count == 0)
        {
            throw new ArgumentException("Section must have at least one collection", nameof(Collections));
        }
    }
}
