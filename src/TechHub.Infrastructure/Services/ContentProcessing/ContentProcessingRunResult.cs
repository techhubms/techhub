namespace TechHub.Infrastructure.Services.ContentProcessing;

/// <summary>
/// Outcome of <see cref="ContentProcessingService.RunAsync"/>.
/// </summary>
/// <param name="JobStarted">Whether a job was started; false when a scheduled check found no due feeds.</param>
/// <param name="ContentChanged">Whether the run added content items, so cached content is stale.</param>
public readonly record struct ContentProcessingRunResult(bool JobStarted, bool ContentChanged);
