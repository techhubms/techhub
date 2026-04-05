using TechHub.Core.Models.ContentProcessing;

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
    /// </summary>
    /// <param name="weekStart">Monday of the week to generate (Europe/Brussels).</param>
    /// <param name="weekEnd">Sunday of the week to generate (Europe/Brussels).</param>
    /// <param name="progress">Optional progress reporter for streaming status messages to callers.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Outcome indicating whether a roundup was generated (including its slug) or why it was skipped.</returns>
    Task<RoundupGenerationOutcome> GenerateAsync(DateOnly weekStart, DateOnly weekEnd, IProgress<string>? progress = null, CancellationToken ct = default);
}
