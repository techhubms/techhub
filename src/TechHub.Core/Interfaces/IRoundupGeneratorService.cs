namespace TechHub.Core.Interfaces;

/// <summary>
/// Service that orchestrates the weekly roundup generation pipeline.
/// Reads candidate articles from the database, runs the multi-step AI pipeline,
/// and writes the generated roundup directly to <c>content_items</c>.
/// </summary>
public interface IRoundupGeneratorService
{
    /// <summary>
    /// Generates a weekly roundup for the given date range and writes it to the database.
    /// Skips generation if a roundup for this week already exists.
    /// </summary>
    /// <param name="weekStart">Monday of the week to generate (Europe/Brussels).</param>
    /// <param name="weekEnd">Sunday of the week to generate (Europe/Brussels).</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>True if a new roundup was generated; false if one already existed.</returns>
    Task<bool> GenerateAsync(DateOnly weekStart, DateOnly weekEnd, CancellationToken ct = default);
}
