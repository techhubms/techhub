using System.Text.Json;

namespace TechHub.Core.Models;

/// <summary>
/// Keyset pagination cursor for stable, performant paging.
/// Encodes the sort key values of the last item on the page.
/// </summary>
public record PaginationCursor
{
    /// <summary>
    /// Unix timestamp of the last item (primary sort key).
    /// </summary>
    public long DateEpoch { get; init; }

    /// <summary>
    /// ID/slug of the last item (tiebreaker for items with same date).
    /// </summary>
    public string Id { get; init; } = "";

    /// <summary>
    /// Encode cursor to Base64 string for URL transmission.
    /// </summary>
    public static string Encode(PaginationCursor cursor) =>
        Convert.ToBase64String(JsonSerializer.SerializeToUtf8Bytes(cursor));

    /// <summary>
    /// Decode cursor from Base64 string.
    /// Returns null if token is null or invalid.
    /// </summary>
    public static PaginationCursor? Decode(string? token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return null;
        }

        try
        {
            return JsonSerializer.Deserialize<PaginationCursor>(
                Convert.FromBase64String(token));
        }
        catch
        {
            return null; // Invalid cursor - graceful degradation to first page
        }
    }
}
