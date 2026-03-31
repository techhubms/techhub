namespace TechHub.Core.Models.Admin;

/// <summary>
/// A custom page data entry stored in the database.
/// Each entry holds the raw JSON for a specific custom page (e.g. "levels", "handbook").
/// </summary>
public sealed record CustomPageEntry
{
    public required string Key { get; init; }
    public required string Description { get; init; }
    public string JsonData { get; init; } = string.Empty;
    public DateTimeOffset UpdatedAt { get; init; }
}
